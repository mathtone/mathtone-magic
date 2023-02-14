using Microsoft.Build.Construction;

namespace Mathtone.Sdk.PackageUtilities.Services {
	public class SolutionAnalysis {
		public SolutionFile? Solution { get; set; }
		public Dictionary<string, ProjectAnalysis> AllProjects { get; set; } = new();
		//public List<List<ProjectAnalysis>> BuildGenerations { get; set; } = new();
	}
}