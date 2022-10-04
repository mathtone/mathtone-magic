using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Runtime.Versioning;

namespace _Sandbox.Logging.Console {
	[UnsupportedOSPlatform("browser")]
	[ProviderAlias("Console")]

	public sealed class ConsoleLoggerProvider : ILoggerProvider {

		private readonly IDisposable _onChangeToken;
		private ConsoleLoggerConfig _currentConfig;
		private readonly ConcurrentDictionary<string, ConsoleLogger> _loggers =
			new(StringComparer.OrdinalIgnoreCase);

		public ConsoleLoggerProvider(IOptionsMonitor<ConsoleLoggerConfig> config) {
			_currentConfig = config.CurrentValue;
			_onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);
		}

		private ConsoleLoggerConfig GetCurrentConfig() =>
			_currentConfig;

		public ILogger CreateLogger(string categoryName) =>
			_loggers.GetOrAdd(categoryName, name => new ConsoleLogger(name, GetCurrentConfig));

		public void Dispose() {
			_loggers.Clear();
			_onChangeToken.Dispose();
		}
	}
}