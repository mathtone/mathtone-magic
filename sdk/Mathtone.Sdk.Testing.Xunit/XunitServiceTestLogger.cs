using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Mathtone.Sdk.Testing.Xunit {
	public class XunitServiceTestLogger : ILogger {

		public XunitServiceTestLogger(string name, Func<XunitTestLoggerLoggerConfiguration> configSelector, ITestOutputHelper output) {
			(_name, _configSelector) = (name, configSelector);
			Output = output;
		}

		private readonly string _name;
		private readonly Func<XunitTestLoggerLoggerConfiguration> _configSelector;
		protected ITestOutputHelper Output { get; }

		public IDisposable? BeginScope<TState>(TState state) where TState : notnull =>
			default!;

		public bool IsEnabled(LogLevel logLevel) => _configSelector()
			.LogLevelToPrefix
			.ContainsKey(logLevel);

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) {
			var config = _configSelector();
			Output.WriteLine($"[{config.LogLevelToPrefix[logLevel]}] {formatter(state, exception)}");
		}
	}
}