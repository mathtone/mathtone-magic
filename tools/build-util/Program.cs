using Mathtone.Sdk.Logging.Console;
using Microsoft.Extensions.Logging;

namespace Build_Util {
	public static class Program {
		static async Task Main(string[] args) {
			await new SolutionAnalysis(new ConsoleLogger(), args[0]).Analyze();
		}
	}

	public class SolutionAnalysis {

		ILogger _log;
		string _solutionFile;

		public SolutionAnalysis(ILogger log, string solutionFile) {
			_log = log;
			_solutionFile = solutionFile;
		}

		public Task Analyze() {
			return Task.CompletedTask;
		}
	}
}