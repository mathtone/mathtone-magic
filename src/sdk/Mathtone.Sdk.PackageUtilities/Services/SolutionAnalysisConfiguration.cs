namespace Mathtone.Sdk.PackageUtilities.Services {
	public class SolutionAnalysisConfiguration {

		public string DefaultVersionMask { get; set; } = "1.0";
		public bool IsPreRelease { get; set; } = true;
		public bool SyncVersion { get; set; } = false;
		public string SolutionFilePath { get; set; } = "";
		public PackageConfig[] Packages { get; set; } = Array.Empty<PackageConfig>();
		public string[] PreReleaseProgression { get; set; } = new[] {
			"alpha",
			"beta",
			"rc*"
		};
		public string PackageOutput { get; set; } = "./packages";
		public string PackScriptLocation { get; set; } = "./create-packages.sh";
		public string PackConfig { get; set; } = "Release";
		public string CompareBranch { get; set; } = "main";
	}
}