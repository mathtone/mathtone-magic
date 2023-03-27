using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using Xunit.Abstractions;

namespace Mathtone.Sdk.Testing.Xunit {
	[ProviderAlias("Xunit")]
	public sealed class XunitTestLoggerProvider : ILoggerProvider {

		private readonly IDisposable? _onChangeToken;
		private XunitTestLoggerLoggerConfiguration _currentConfig;
		private readonly ConcurrentDictionary<string, XunitServiceTestLogger> _loggers = new(StringComparer.OrdinalIgnoreCase);
		protected readonly ITestOutputHelper _output;

		public XunitTestLoggerProvider(IOptionsMonitor<XunitTestLoggerLoggerConfiguration> config, ITestOutputHelper output) {
			_currentConfig = config.CurrentValue;
			_onChangeToken = config.OnChange(cfg => _currentConfig = cfg);
			_output = output;
		}

		public ILogger CreateLogger(string categoryName) =>
			 _loggers.GetOrAdd(categoryName, name => new XunitServiceTestLogger(name, GetCurrentConfig, _output));

		private XunitTestLoggerLoggerConfiguration GetCurrentConfig() => _currentConfig;

		public void Dispose() {
			_loggers.Clear();
			_onChangeToken?.Dispose();
		}
	}
}