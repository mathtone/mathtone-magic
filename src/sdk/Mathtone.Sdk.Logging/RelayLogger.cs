using Microsoft.Extensions.Logging;

namespace Mathtone.Sdk.Logging {
	public class RelayLogger : Logger {

		readonly LogHandler _handler;

		public RelayLogger(LoggerExternalScopeProvider scopeProvider, string categoryName, LogHandler handler) : base(scopeProvider, categoryName) {
			_handler = handler;
		}

		public override bool IsEnabled(LogLevel logLevel) {
			throw new NotImplementedException();
		}

		protected override void OnWrite(LogLevel level, EventId eventId, Exception? exception, string message) =>
			_handler(level, eventId, exception, message);
	}
}