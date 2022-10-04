using Microsoft.Extensions.Logging;

namespace Mathtone.Sdk.Testing.Xunit.Tests.Logging.Console {
	public class ColorConsoleLoggerConfiguration {
		public int EventId { get; set; }

		public Dictionary<LogLevel, ConsoleColor> LogLevelToColorMap { get; set; } = new() {
			[LogLevel.Information] = ConsoleColor.Green
		};
	}


}