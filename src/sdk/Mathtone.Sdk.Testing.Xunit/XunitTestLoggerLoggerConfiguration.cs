using Microsoft.Extensions.Logging;

namespace Mathtone.Sdk.Testing.Xunit {
	public sealed class XunitTestLoggerLoggerConfiguration {
		public Dictionary<LogLevel, string> LogLevelToPrefix { get; set; } = new() {
			{LogLevel.None, "" },
			{LogLevel.Trace, "TRC" },
			{LogLevel.Debug, "DBG" },
			{LogLevel.Information, "INF" },
			{LogLevel.Warning, "WAR" },
			{LogLevel.Error, "ERR" },
			{LogLevel.Critical, "CRI" },
		};
	}
}