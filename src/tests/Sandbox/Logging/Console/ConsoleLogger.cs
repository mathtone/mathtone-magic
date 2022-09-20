using Microsoft.Extensions.Logging;
using static System.Console;

namespace _Sandbox.Logging.Console {

	public class ConsoleLogger : ILogger {

		private readonly string _name;
		private readonly Func<ConsoleLoggerConfig> _getCurrentConfig;
		public IDisposable BeginScope<TState>(TState state) => default!;

		public ConsoleLogger(string name, Func<ConsoleLoggerConfig> getCurrentConfig) =>
			(_name, _getCurrentConfig) = (name, getCurrentConfig);

		public bool IsEnabled(LogLevel logLevel) => true;

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) {

			if (!IsEnabled(logLevel)) {
				return;
			}

			ConsoleLoggerConfig config = _getCurrentConfig();

			if (config.EventId == 0 || config.EventId == eventId.Id) {
				WriteLine($"[{eventId.Id,2}: {logLevel,-12}]");
				Write($"     {_name} - ");
				Write($"{formatter(state, exception)}");
				WriteLine();
			}
		}
	}
}