using Mathtone.Sdk.Logging;
using Mathtone.Sdk.Utilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Mathtone.Sdk.Testing.Xunit {
	
	public class XunitLogger : Logger {

		readonly ITestOutputHelper _output;

		public XunitLogger(ITestOutputHelper output, LoggerExternalScopeProvider scopeProvider, string categoryName) :
			base(scopeProvider, categoryName) {
			_output = output;
		}

		public override bool IsEnabled(LogLevel logLevel) {
			throw new NotImplementedException();
		}

		protected override void OnWrite(LogLevel level, EventId eventId, Exception? exception, string message) =>
			_output.WriteLine(message);
	}


	public class XunitLogger<T> : XunitLogger, ILogger<T> {
		public XunitLogger(ITestOutputHelper output, LoggerExternalScopeProvider scopeProvider)
			: base(output, scopeProvider, typeof(T).FullName!) {
		}
	}
}