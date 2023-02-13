using Mathtone.Sdk.Secrets;
using Mathtone.Sdk.Services;
using Microsoft.Build.Construction;
using Microsoft.Extensions.Logging;

using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;

namespace Mathtone.Sdk.PackageUtilities.Services {

	public class PackageConfig {
		public string Name { get; set; } = "";
		public string? VersionMask { get; set; }
	}

	//TODO: Push this wreckage off the deck and write it better.
	public partial class SolutionAnalysisService : LoggedServiceBase {
		public SolutionAnalysisService(ILogger<SolutionAnalysisService> logger, ISecrets secrets, SolutionAnalysisConfiguration config) : base(logger) {
			Secrets = secrets;
			Config = config;
		}

		ISecrets Secrets { get; }
		public SolutionAnalysisConfiguration Config { get; }
		string SolutionPath => Config.SolutionFilePath;
		SolutionAnalysis? CurrentAnalysis { get; set; }
		IEnumerable<ProjectAnalysis> AllProjects => CurrentAnalysis!.AllProjects.Values;

		public virtual async Task<SolutionAnalysis> Analyze() {
			LogAnalysisBegun("Solution", SolutionPath);
			var sln = SolutionFile.Parse(Path.GetFullPath(SolutionPath));
			CurrentAnalysis = new() {
				Solution = sln,
				AllProjects = GetProjects(sln).ToDictionary(p => p.ProjectName, p => new ProjectAnalysis(p))
			};

			foreach (var p in AllProjects) {
				var xml = new XmlDocument();
				xml.LoadXml(await File.ReadAllTextAsync(p.Project.AbsolutePath));
				LogAnalysisBegun("Project", p.Project.ProjectName);
				foreach (var v in xml
					.GetElementsByTagName("ProjectReference")
					.Cast<XmlElement>()
					.Select(e => GetProjectName(e.GetAttribute("Include")))) {

					var refProj = CurrentAnalysis.AllProjects[v];
					LogDependency(p.Project.ProjectName, refProj.Project.ProjectName);
					refProj.Dependencies.Add(p.Dependencies);
				}
				LogAnalysisComplete("Project", p.Project.ProjectName);
			}

			AssignBuildGenerations();

			await FindProjectChanges();
			await AssignBuildVersions();
			await AssignNewVersions();
			await CreatePackageScript();
			//Create Build Scripts

			LogAnalysisComplete("Solution", SolutionPath);
			return CurrentAnalysis;
		}

		protected virtual async Task AssignBuildVersions() {
			var packageSource = new PackageSource("https://mathtone.jfrog.io/artifactory/api/nuget/v3/mathtone-dev") {
				Credentials = new PackageSourceCredential(
					"https://mathtone.jfrog.io/artifactory/api/nuget/v3/mathtone-dev",
					Secrets["ARTIFACTORY_USER"],
					Secrets["ARTIFACTORY_PWD"],
					true,
					null
				)
			};

			var cache = new SourceCacheContext();
			var repository = Repository.Factory.GetCoreV3(packageSource);
			var resource = await repository.GetResourceAsync<MetadataResource>();

			await Task.WhenAll(AllProjects.Select(p => Task.Run(async () => {
				var n = p.Project.ProjectName;
				p.CurrentPreReleaseVersion = await resource.GetLatestVersion(n, true, true, cache, NuGet.Common.NullLogger.Instance, CancellationToken.None);
				p.CurrentReleaseVersion = await resource.GetLatestVersion(n, false, true, cache, NuGet.Common.NullLogger.Instance, CancellationToken.None);
				Log.LogInformation("PACKAGE: {pkg} VERSION: {pver}/{rver}", n, p.CurrentPreReleaseVersion, p.CurrentReleaseVersion);
			})));
		}

		protected virtual async Task FindProjectChanges() {

			foreach (var gen in AllProjects.GroupBy(p => p.BuildGeneration).OrderBy(g => g.Key)) {
				foreach (var p in gen) {
					;
					if (!p.HasChanged) {

						var proc = new Process();

						proc.StartInfo.UseShellExecute = false;
						proc.StartInfo.WorkingDirectory = Path.GetDirectoryName(p.Project.AbsolutePath);
						proc.StartInfo.RedirectStandardOutput = true;
						proc.StartInfo.RedirectStandardError = true;
						proc.StartInfo.FileName = "git";
						proc.StartInfo.Arguments = $"diff --name-only {Config.CompareBranch} -- ./";
						proc.Start();

						var output = proc.StandardOutput.ReadToEnd();
						var err = proc.StandardError.ReadToEnd();

						if (err != "") {
							Log.LogError("GIT DIFF ERROR: {proj} - {msg}", p.Project.ProjectName, err);
						}
						else {
							if (output == "") {
								Log.LogInformation("GIT DIFF: {proj} - UNCHANGED", p.Project.ProjectName);
							}
							else {
								Log.LogInformation("GIT DIFF: {proj} - CHANGED", p.Project.ProjectName);
								foreach (var px in p.Dependencies.WithDescendents) {
									Log.LogInformation("- {proj}", px.Value!.Project.ProjectName);
									px.Value!.HasChanged = true;
								}
							}
						}

						await proc.WaitForExitAsync();
					}
				}
			}
		}

		protected virtual Task AssignNewVersions() {

			var defaultMask = Config.DefaultVersionMask.Split(".").Select(int.Parse).ToArray();

			foreach (var cfg in Config.Packages) {

				var p = CurrentAnalysis!.AllProjects[cfg.Name];
				if (p.HasChanged) {

					var cur = p.CurrentPreReleaseVersion ?? p.CurrentReleaseVersion;
					var upd = GetNewVersion(cur, defaultMask[0], defaultMask[1], Config.PreReleaseProgression, !Config.IsPreRelease);
					p.NewReleaseVersion= upd;
					LogProjectVersion(cfg.Name, upd.ToString());
				}
				else {
					LogProjectAction(cfg.Name, "Unchanged");
				}
			}

			return Task.CompletedTask;
		}

		protected virtual void AssignBuildGenerations() {
			var g = 0;
			var gen = CurrentAnalysis!.AllProjects.Values.Where(p => p.BuildGeneration == g).ToArray();
			while (gen.Any()) {
				LogGeneration(g);
				foreach (var project in gen) {
					if (project.Dependencies.Parents.Select(p => p.Value).Any(p => gen.Contains(p))) {
						project.BuildGeneration++;
					}
					else {
						LogProjectName(project.Project.ProjectName);
					}
				}
				g++;
				gen = CurrentAnalysis.AllProjects.Values.Where(p => p.BuildGeneration == g).ToArray();
			}
		}

		protected virtual async Task CreatePackageScript() {
			var sb = new StringBuilder();

			foreach (var gen in CurrentAnalysis!.AllProjects.Values
				.Where(p => p.HasChanged && Config.Packages.Any(c => c.Name == p.Project.ProjectName))
				.GroupBy(p => p.BuildGeneration).OrderBy(p => p.Key)) {
				
				sb.AppendLine($"#GEN: {gen.Key}");
				//Log.LogInformation("Generation {generation}", gen.Key);

				foreach (var proj in gen) {
					var sfx = Config.IsPreRelease ? "--version-suffix" : "";
					var cmd = $"dotnet pack \"{proj.Project.AbsolutePath}\" {sfx} {proj.NewReleaseVersion} -c {Config.PackConfig} --output {Config.PackageOutput}";
					sb.AppendLine(cmd);
				}
			}

			await File.WriteAllTextAsync(Config.PackScriptLocation, sb.ToString());
			Log.LogInformation("{file}", Path.GetFullPath(Config.PackScriptLocation));
		}

		public static NuGetVersion GetNewVersion(NuGetVersion currentVersion, int major, int minor, string[] releaseProgression, bool release) {

			var rev = currentVersion?.Patch ?? 0;
			var rel = currentVersion?.Release;
			var isNew = currentVersion == null;

			var verChange = currentVersion == null || currentVersion.Major != major || currentVersion.Minor != minor;

			if (release) {
				
				if (currentVersion?.IsPrerelease ?? true) {
					rel = "";
				}
				else {
					rev += 1;
					rel = "";
				}
			}
			else {
				if (isNew) {
					rel = releaseProgression[0];
				}
				else if (currentVersion.IsPrerelease) {
					;
					rel = NextRelease(rel, releaseProgression);
				}
				else {
					if (!verChange) {
						rev += 1;
					}
					rel = releaseProgression[0];
				}
			}
			return new(major, minor, rev, rel);
		}

		public static string NextRelease(string currentRelease, string[] releaseProgression) {
			if ((currentRelease) == "") return releaseProgression[0];
			var i = Array.IndexOf(releaseProgression, currentRelease) + 1;

			if (i == 0) {
				var l = releaseProgression.Last();
				;
				var x = int.Parse(currentRelease.Replace(l, "")) + 1;
				return l + x.ToString();
			}
			else {
				if (i >= releaseProgression.GetUpperBound(0)) {
					return releaseProgression[i] + "1";
				}
				else {
					return releaseProgression[i];
				}
			}
		}

		public static IEnumerable<ProjectInSolution> GetProjects(SolutionFile solution) => solution
			.ProjectsInOrder
			.Where(p => p.ProjectType != SolutionProjectType.SolutionFolder);

		public static string GetProjectName(string projectPath) {
			var splitChar = '\\';
			if (projectPath.Contains('/')) {
				splitChar = '/';
			}
			return Path.GetFileNameWithoutExtension(projectPath.Split(splitChar).Last());
		}

		[LoggerMessage(0, LogLevel.Information, "{message}")]
		partial void LogProjectName(string message);

		[LoggerMessage(1, LogLevel.Information, "Analyzing {type}:{name}")]
		partial void LogAnalysisBegun(string type, string name);

		[LoggerMessage(2, LogLevel.Information, "{type}:{name} Analysis Complete")]
		partial void LogAnalysisComplete(string type, string name);

		[LoggerMessage(3, LogLevel.Information, "{projects} Projects Processed")]
		partial void LogProjectsProcessed(int projects);

		[LoggerMessage(4, LogLevel.Information, "{project1} Depends On {project2}")]
		partial void LogDependency(string project1, string project2);

		[LoggerMessage(5, LogLevel.Information, "Generation: {gen}")]
		partial void LogGeneration(int gen);

		[LoggerMessage(6, LogLevel.Information, "Project: {project} - {action}")]
		partial void LogProjectAction(string project, string action);

		[LoggerMessage(7, LogLevel.Information, "Project: {project} Updating to: {version}")]
		partial void LogProjectVersion(string project, string version);
	}
}