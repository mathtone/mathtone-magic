using Mathtone.Sdk.Graphs;
using Mathtone.Sdk.Services;
using Mathtone.Sdk.Testing.Xunit;
using Microsoft.Build.Construction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using Xunit.Abstractions;

namespace Mathtone.Sdk.BuildUtilities.Tests {
	public class UnitTest1 : XunitServiceTestBase {

		public UnitTest1(ITestOutputHelper output) :
			base(output) { }

		[Fact]
		public async Task SolutionAnalyzerTests() {
			var tree = await SolutionAnalyzer.GetProjectTree("../../../../../mathtone-magic.sln");
			var sb = new StringBuilder();
			
			PrintTree(tree,sb);
			LogMessage(sb.ToString());
		}

		void PrintTree(Tree<ProjectInSolution> tree, StringBuilder sb) {
			var prefix = "".PadRight(tree.WithAncestors.Count(), '.');
			sb.Append(prefix);
			sb.AppendLine(tree.Value?.ProjectName);
			foreach(var n in tree) {
				PrintTree(n, sb);
			}
		}
		
		[Fact]
		public async Task Test1() => await Services
			.Activated<ISolutionProcessor, SolutionProcessorConfig>(new() {
				SolutionFilePath = "../../../../../mathtone-magic.sln"
			})
			.Run();

		protected override IServiceCollection OnConfigureServices(IServiceCollection services) => base
			.OnConfigureServices(services)
			.AddActivation<ISolutionProcessor, SolutionProcessor, SolutionProcessorConfig>()
			.AddSingleton<SolutionProcessor>();
	}

	public static class SolutionAnalyzer {

		public static async Task<Tree<ProjectInSolution>> GetProjectTree(string solutionFilePath) {
			var d = GetProjects(solutionFilePath)
				.Select(p => new Tree<ProjectInSolution>(p))
				.ToDictionary(t => t.Value!.ProjectName);

			foreach (var project in d.Values) {

				var xml = new XmlDocument();
				xml.LoadXml(await File.ReadAllTextAsync(project.Value!.AbsolutePath));

				foreach (var v in xml
					.GetElementsByTagName("ProjectReference")
					.Cast<XmlElement>()
					.Select(e => GetProjectName(e.GetAttribute("Include")))) {

					d[v].Add(project);
				}
			}

			var roots = d.Values.Where(n => n.Parent == null).ToArray();
			if (roots.Length == 1) {
				return roots[0];
			}
			else {
				var rtn = new Tree<ProjectInSolution>();
				foreach (var r in roots) {
					rtn.Add(r);
				}
				return rtn;
			}
		}

		public static IEnumerable<ProjectInSolution> GetProjects(string solutionFile) => SolutionFile
			.Parse(Path.GetFullPath(solutionFile))
			.ProjectsInOrder
			.Where(p => p.ProjectType != SolutionProjectType.SolutionFolder);

		public static string GetProjectName(string projectPath) {
			var splitChar = '\\';
			if (projectPath.Contains('/')) {
				splitChar = '/';
			}
			return Path.GetFileNameWithoutExtension(projectPath.Split(splitChar).Last());
		}
	}

	public class SolutionProcessor : LoggedServiceBase, ISolutionProcessor {
		public SolutionProcessor(ILogger<SolutionProcessor> logger, SolutionProcessorConfig config) :
			base(logger) {
			Config = config;
		}

		public SolutionProcessorConfig Config { get; }

		public async Task Run() {
			var tree = await SolutionAnalyzer.GetProjectTree(Config.SolutionFilePath);
			;
		}
	}

	public interface ISolutionProcessor {
		Task Run();
	}
}