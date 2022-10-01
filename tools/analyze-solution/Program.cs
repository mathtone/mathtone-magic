// See https://aka.ms/new-console-template for more information

using CommandLine;
using Mathtone.Sdk.Common.Extensions;
using Mathtone.Sdk.Logging.Console;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using System.Security;
using System.Xml;

namespace AnalyzeSolution {

	public static class Program {
		public static async Task<int> Main(string[] args) {
			var p = Path.GetFullPath(args[0]);
			if (File.Exists(p)) {
				Console.WriteLine("FOUND SOLUTION");
				await new SolutionProcessor(new ConsoleLogger(), p).Process();
				return 0;
			}
			else {
				Console.WriteLine("NO SOLUTION");
				return -1;
			}
		}
	}

	public class SolutionProcessor {

		ILogger _log;
		string _solution;
		Dictionary<string, ProjectInSolution>? _sdkProjects;
		TextWriter? _removals;
		TextWriter? _additions;

		public SolutionProcessor(ILogger log, string solution) {
			_log = log;
			_solution = solution;
		}

		public async Task Process() {
			_log.LogInformation($"PROCESSING SOLUTION: {_solution}");
			var sln = SolutionFile.Parse(_solution);

			_sdkProjects = sln.ProjectsInOrder
				.Where(p =>
					p.ProjectType != SolutionProjectType.SolutionFolder &&
					Path.GetRelativePath(Path.GetDirectoryName(_solution)!, p.AbsolutePath).IsSubPathOf("sdk")
				).ToDictionary(p => p.ProjectName);

			await Task.WhenAll(
				sln.ProjectsInOrder
					.Where(p => p.ProjectType != SolutionProjectType.SolutionFolder)
					.Select(p => ProcessProject(p.AbsolutePath))
			);
			_log.LogInformation($"PROCESSING COMPLETE: {_solution}");
		}

		protected async Task ProcessProject(string projectFile) {

			var p = Path.GetRelativePath(Path.GetDirectoryName(_solution)!, projectFile).IsSubPathOf("sdk");
			var doc = new XmlDocument();
			var xml = await File.ReadAllTextAsync(projectFile);
			doc.LoadXml(xml);
			var refs = doc.GetElementsByTagName("ProjectReference");
			foreach (XmlElement r in refs) {
				var referencedProject = Path.GetFileNameWithoutExtension(r.GetAttribute("Include"));
				if (_sdkProjects!.ContainsKey(referencedProject)) {
					_log.LogInformation($" -References: {referencedProject}");
					var px = Project.FromFile(projectFile, new());

					;
				}
			}
			_log.LogInformation($"{projectFile} LOADED");
		}
	}

	static class PathExtensions {
		public static bool IsSubPathOf(this string subPath, string basePath) {
			var rel = Path.GetRelativePath(basePath, subPath);
			return rel != "."
				&& rel != ".."
				&& !rel.StartsWith("../")
				&& !rel.StartsWith(@"..\")
				&& !Path.IsPathRooted(rel);
		}
	}

	//public class CommandLineOptions {
	//	[Value(index: 0, Required = true, HelpText = "Solution file Path to analyze.")]
	//	public string? SolutionFile { get; set; }
	//}

	//public static class SolutionProcessor {

	//	static IDictionary<string, string> Packages = new Dictionary<string, string>() {
	//		{"Mathtone.Sdk.Common","Mathtone.Sdk.Common" },
	//		{"Mathtone.Sdk.Logging","Mathtone.Sdk.Logging" },
	//		{"Mathtone.Sdk.Utilities","Mathtone.Sdk.Utilities" },
	//		{"Mathtone.Sdk.Data","Mathtone.Sdk.Data" },
	//		{"Mathtone.Sdk.Data.Sql","Mathtone.Sdk.Data.Sql" },
	//		{"Mathtone.Sdk.Data.Npgsql","Mathtone.Sdk.Data.Npgsql" },
	//		{"Mathtone.Sdk.Data.Npgsql","Mathtone.Sdk.Testing.Xunit" }
	//	};

	//	public static Task Process(string solutionFile) {
	//		var sln = SolutionFile.Parse(solutionFile);
	//		foreach (var proj in sln.ProjectsInOrder) {
	//			if (proj.ProjectType != SolutionProjectType.SolutionFolder) {
	//				var p = Microsoft.Build.Construction.ProjectRootElement.Open(solutionFile);
	//				;
	//				//var p = Project.FromFile(proj.AbsolutePath, new() { });
	//				//;
	//			}
	//		}
	//		return Task.CompletedTask;
	//	}
	//}

	//public class PackageMap {
	//	public string Project { get; set; }
	//	public string Package { get; set; }

	//	public PackageMap(string project, string? package = default) {
	//		Project = project;
	//		Package = package ?? project;
	//	}
	//}

	//public class Program {
	//	static async Task<int> Main(string[] args) => await Parser.Default
	//		.ParseArguments<CommandLineOptions>(args)
	//		.MapResult(async (CommandLineOptions opts) => {
	//			try {
	//				// We have the parsed arguments, so let's just pass them down
	//				await SolutionProcessor.Process(opts.SolutionFile!);
	//				return 0;
	//			}
	//			catch {
	//				Console.WriteLine("Error!");
	//				return -3; // Unhandled error
	//			}
	//		},
	//		errs => Task.FromResult(-1)); // Invalid arguments

	//	//static async Task AnalyzeSolution(string solution) {
	//	//	if (!File.Exists(solution)) {
	//	//		throw new FileNotFoundException(solution);
	//	//	}

	//	//	await Task.WhenAll(SolutionFile.Parse(solution).ProjectsInOrder
	//	//		.Where(p => p.ProjectType != SolutionProjectType.SolutionFolder)
	//	//		.Select(ProcessProject));
	//	//}

	//	//static 

	//	//static async Task ProcessProject(ProjectInSolution project) {
	//	//	Console.WriteLine(project.RelativePath);
	//	//	Console.WriteLine(project.ProjectName);
	//	//	await Task.CompletedTask;
	//	//}
	//}
}