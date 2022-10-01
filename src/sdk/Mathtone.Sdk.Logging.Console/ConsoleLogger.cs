using Microsoft.Extensions.Logging;
using System;
using static System.Formats.Asn1.AsnWriter;

namespace Mathtone.Sdk.Logging.Console {
	public class ConsoleLoggerConfig {
		public int EventId { get; set; }
	}

	public class ConsoleLogger : Logger {

		public ConsoleLogger() : base() { }
		public ConsoleLogger(LoggerExternalScopeProvider scopeProvider, string categoryName) : base(scopeProvider, categoryName) { }

		public override bool IsEnabled(LogLevel logLevel) => true;
		protected override void OnWrite(LogLevel level, EventId eventId, Exception? exception, string message) =>
			System.Console.Write(message);
	}
}