using Microsoft.Extensions.Logging;
using static System.Console;

namespace _Sandbox.Logging.Console {
	public sealed class ColorConsoleLogger : ILogger {
		private readonly string _name;
		private readonly Func<ColorConsoleLoggerConfiguration> _getCurrentConfig;

		public ColorConsoleLogger(
			string name,
			Func<ColorConsoleLoggerConfiguration> getCurrentConfig) =>
			(_name, _getCurrentConfig) = (name, getCurrentConfig);

		public IDisposable BeginScope<TState>(TState state) => default!;

		public bool IsEnabled(LogLevel logLevel) =>
			_getCurrentConfig().LogLevelToColorMap.ContainsKey(logLevel);

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) {

			if (!IsEnabled(logLevel)) {
				return;
			}

			ColorConsoleLoggerConfiguration config = _getCurrentConfig();
			if (config.EventId == 0 || config.EventId == eventId.Id) {
				ConsoleColor originalColor = ForegroundColor;

				ForegroundColor = config.LogLevelToColorMap[logLevel];
				WriteLine($"[{eventId.Id,2}: {logLevel,-12}]");

				ForegroundColor = originalColor;
				Write($"     {_name} - ");

				ForegroundColor = config.LogLevelToColorMap[logLevel];
				Write($"{formatter(state, exception)}");

				ForegroundColor = originalColor;
				WriteLine();
			}
		}
	}
}