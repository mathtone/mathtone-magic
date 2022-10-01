using Mathtone.Sdk.Logging.Console;
using Microsoft.Build.Construction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Xml;

namespace Build_Util {
	public static class Program {
		static async Task Main(string[] args) {
			var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/appsettings.json")
				.Build()
				.GetSection("Settings").Get<SolutionAnalysisConfig>()!;

			await new SolutionAnalyzer(new ConsoleLogger(), args[0], config).Analyze();
			;
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
	}

	//public class SolutionAnalysis {

	//}

	public class SolutionAnalyzer {

		readonly ILogger _log;
		readonly string _solutionFile;
		readonly SolutionAnalysisConfig _config;
		readonly Dictionary<string, ProjectDependencies> _projects;

		public SolutionAnalyzer(ILogger log, string solutionFile, SolutionAnalysisConfig config) {
			_log = log;
			_solutionFile = solutionFile;
			_config = config;
			_projects = GetProjects(_solutionFile).ToDictionary(p => p.ProjectName, p => new ProjectDependencies(p));
		}

		public async Task Analyze() {
			await Task.WhenAll(_projects.Values.Select(AddProjectDependencies));
			var gen0 = _projects.Values.ToDictionary(p=>p.Project.ProjectName);
			//var gen0 = _projects.ToDictionary(p => p.Value.Project.ProjectName);
			var generations = new List<Dictionary<string, ProjectDependencies>>() {
				{gen0}
				//{_projects.Values.Where(p => p.Dependencies.Any(p=>gen0.ContainsKey(p.ProjectName))).ToDictionary(p => p.Project.ProjectName) }
			};
			var g = generations[0];
			while (g.Any()) {
				g = PromoteToGeneration(g);
				if(g.Any())
					generations.Add(g);
			}
			var i = 0;
			foreach(var gn in generations) {
				_log.LogInformation("Generation: {gen}", i++);
				foreach(var pj in gn.Values)
				_log.LogInformation(" - {proj}",pj.Project.ProjectName );
			}
			await Task.CompletedTask;
		}

		protected Dictionary<string,ProjectDependencies> PromoteToGeneration(Dictionary<string,ProjectDependencies> currentGen) {
			var rtn = currentGen.Values.Where(p => p.Dependencies.Any(p => currentGen.ContainsKey(p.ProjectName))).ToDictionary(p => p.Project.ProjectName);
			foreach(var k in rtn.Keys) {
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
				.Select(p=>LocateProject(p).Project);
			project.Dependencies.AddRange(dependencies);
		}

		protected ProjectDependencies LocateProject(string projectPath) {
			return _projects[Path.GetFileNameWithoutExtension(Path.GetFileName(projectPath))];
		}

		static IEnumerable<ProjectInSolution> GetProjects(string solutionFile) => SolutionFile.Parse(Path.GetFullPath(solutionFile))
			.ProjectsInOrder
			.Where(p => p.ProjectType != SolutionProjectType.SolutionFolder);
	}
}