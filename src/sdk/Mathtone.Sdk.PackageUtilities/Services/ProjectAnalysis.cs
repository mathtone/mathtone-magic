using Mathtone.Sdk.Graphs;
using Microsoft.Build.Construction;
using NuGet.Versioning;

namespace Mathtone.Sdk.PackageUtilities.Services {
	public class ProjectAnalysis {

		public ProjectAnalysis(ProjectInSolution project) {
			Project = project;
			Dependencies = new(this);
		}
		public bool HasChanged { get; set; }
		public NuGetVersion CurrentReleaseVersion { get; set; } = default!;
		public NuGetVersion CurrentPreReleaseVersion { get; set; } = default!;
		public NuGetVersion NewReleaseVersion { get; set; } = default!;
		public int BuildGeneration { get; set; }
		public ProjectInSolution Project { get; }
		public Tangle<ProjectAnalysis> Dependencies { get; }
	}
}