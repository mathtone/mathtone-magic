﻿using Mathtone.Sdk.Logging.Console;
using Microsoft.Build.Construction;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.ComponentModel.Design;
using System.Reflection;
using System.Text;
using System.Xml;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Build_Util {
	public static class Program {
		static async Task Main(string[] args) {
			//var i = new 
			var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/packageconfig.json")
				.Build()
				.GetSection("Settings").Get<SolutionAnalysisConfig>()!;

			await new SolutionAnalyzer(new ConsoleLogger(), args[0], config).Analyze();
		}
	}

	public class ProjectDependencies {

		public ProjectInSolution Project { get; set; }
		public List<ProjectInSolution> Dependencies { get; } = new List<ProjectInSolution>();

		public ProjectDependencies(ProjectInSolution project) {
			Project = project;
		}
	}

	public class SolutionAnalysisConfig {
		public string[]? PackageProjects { get; set; }
		public string? PackageDirectory { get; set; }
		public LoggerVerbosity Verbosity { get; set; }
	}


	public class SolutionAnalyzer {

		readonly ILogger _log;
		readonly string _solutionFile;
		readonly SolutionAnalysisConfig _config;
		readonly Dictionary<string, ProjectDependencies> _projects;
		readonly StringBuilder _removeCommand = new();
		readonly StringBuilder _addCommand = new();
		readonly StringBuilder _genCommand = new();

		public SolutionAnalyzer(ILogger log, string solutionFile, SolutionAnalysisConfig config) {
			_log = log;
			_solutionFile = solutionFile;
			_config = config;
			_projects = GetProjects(_solutionFile).ToDictionary(p => p.ProjectName, p => new ProjectDependencies(p));
		}

		public async Task Analyze() {

			await Task.WhenAll(_projects.Values.Select(AddProjectDependencies));
			var packages = _config.PackageProjects!.ToHashSet();
			var generations = new List<Dictionary<string, ProjectDependencies>>() {
				{_projects.Values.ToDictionary(p=>p.Project.ProjectName)}
			};
			List<string> removeCommands = new();
			List<string> addCommands = new();
			List<List<string>> genCommands = new();
			var g = generations[0];
			while (g.Any()) {
				g = PromoteToGeneration(g);
				if (g.Any())
					generations.Add(g);
			}

			for (var i = 0; i < generations.Count; i++) {
				var genCmd = new List<string>();
				var gn = generations[i];
				_log.LogInformation("Generation: {gen}", i);
				var packs = new List<string>();
				var restores = new List<string>();
				foreach (var pj in gn.Values) {
					var pack = packages.Contains(pj.Project.ProjectName);
					_log.LogInformation(" - {proj} Pack: {pack}", pj.Project.ProjectName, pack);
					foreach (var d in pj.Dependencies) {
						if (packages.Contains(d.ProjectName)) {
							var proj = _projects[d.ProjectName];
							_log.LogInformation("  - {dep}", d.ProjectName);
							removeCommands.Add($"dotnet remove {pj.Project.AbsolutePath} reference {d.AbsolutePath}");
							addCommands.Add($"dotnet add {pj.Project.AbsolutePath} package {d.ProjectName} --no-restore --prerelease");
						}
					}
					restores.Add($"dotnet restore {pj.Project.AbsolutePath} -s /mnt/ramdisk/packages --verbosity {(int)_config.Verbosity}");

					if (pack) {
						packs.Add(@$"dotnet pack {pj.Project.AbsolutePath} -o {_config.PackageDirectory} --verbosity {(int)_config.Verbosity} /p:VersionPrefix=$PKG_VER $PKG_SFX_TAG");
					}
				}
				//var r = string.Join($" &{Environment.NewLine}", restores);
				genCommands.Add(new() {
					 string.Join($" &{Environment.NewLine}", restores),
					 string.Join($" &{Environment.NewLine}", packs)
				});
				//genCommands.Add(packs);
			}
			_removeCommand.AppendLine();
			_removeCommand.AppendLine(string.Join($" {Environment.NewLine}", removeCommands));

			_addCommand.AppendLine();
			_addCommand.AppendLine(string.Join($" {Environment.NewLine}", addCommands));

			for (var i = 0; i < genCommands.Count; i++) {
				_genCommand.AppendLine($"echo \"**********GENERATION {i}\"");
				_genCommand.AppendLine(string.Join($" {Environment.NewLine}", genCommands[i]));
				_genCommand.AppendLine($"ls -l /mnt/ramdisk/packages");
			}

			using var f = File.OpenWrite($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/process-sln.sh");

			using var sw = new StreamWriter(f);
			//await sw.WriteAsync("dotnet nuget locals all --clear");
			await sw.WriteLineAsync("********************************************");
			await sw.WriteLineAsync("echo build- ${PKG_VER} version- ${PKG_SFX}");
			await sw.WriteLineAsync("********************************************");
			await sw.WriteLineAsync(_removeCommand.ToString());
			await sw.WriteLineAsync(_addCommand.ToString());
			await sw.WriteLineAsync(_genCommand.ToString());
			await sw.FlushAsync();
			_log.LogInformation("{remove}{add}{gen}", _removeCommand.ToString(), _addCommand.ToString(), _genCommand.ToString());
		}

		protected Dictionary<string, ProjectDependencies> PromoteToGeneration(Dictionary<string, ProjectDependencies> currentGen) {
			var rtn = currentGen.Values.Where(p => p.Dependencies.Any(p => currentGen.ContainsKey(p.ProjectName))).ToDictionary(p => p.Project.ProjectName);
			foreach (var k in rtn.Keys) {
				currentGen.Remove(k);
			}
			return rtn;
		}


		public async Task AddProjectDependencies(ProjectDependencies project) {
			var xml = new XmlDocument();
			xml.LoadXml(await File.ReadAllTextAsync(project.Project.AbsolutePath));
			var dependencies = xml.GetElementsByTagName("ProjectReference")
				.Cast<XmlElement>()
				.Select(e => e.GetAttribute("Include"))
				.Select(p => LocateProject(p).Project)
				.ToArray();

			project.Dependencies.AddRange(dependencies);

		}

		static string GetProjectName(string projectPath) {
			var splitChar = '\\';
			if (projectPath.Contains('/')) {
				splitChar = '/';
			}
			return Path.GetFileNameWithoutExtension(projectPath.Split(splitChar).Last());
		}

		protected ProjectDependencies LocateProject(string projectPath) => _projects[GetProjectName(projectPath)];


		static IEnumerable<ProjectInSolution> GetProjects(string solutionFile) => SolutionFile.Parse(Path.GetFullPath(solutionFile))
			.ProjectsInOrder
			.Where(p => p.ProjectType != SolutionProjectType.SolutionFolder);
	}
}