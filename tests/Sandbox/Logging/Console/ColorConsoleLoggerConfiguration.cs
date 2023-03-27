using Microsoft.Extensions.Logging;

namespace _Sandbox.Logging.Console {
	public class ColorConsoleLoggerConfiguration {
		public int EventId { get; set; }

		public Dictionary<LogLevel, ConsoleColor> LogLevelToColorMap { get; set; } = new() {
			[LogLevel.Information] = ConsoleColor.Green
		};
	}


}