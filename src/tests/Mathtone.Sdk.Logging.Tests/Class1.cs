using Mathtone.Sdk.Common.Utility;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Mathtone.Sdk.Logging.Tests {
	public class XunitOutputAdapter : TextOutputAdapter {
		public XunitOutputAdapter(ITestOutputHelper output) :
			base(output.WriteLine) {
		}
	}

	public class XunitLogger : ILogger {

		private readonly IAsyncTextOutput _output;
		private readonly string _categoryName;
		private readonly LoggerExternalScopeProvider _scopeProvider;

		public static ILogger Create(IAsyncTextOutput output) =>
			new XunitLogger(output, new LoggerExternalScopeProvider(), "");

		public static ILogger<T> Create<T>(IAsyncTextOutput output) =>
			new XunitLogger<T>(output, new LoggerExternalScopeProvider());

		public XunitLogger(IAsyncTextOutput output, LoggerExternalScopeProvider scopeProvider, string categoryName) {
			_output = output;
			_scopeProvider = scopeProvider;
			_categoryName = categoryName;
		}

		public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

		public IDisposable BeginScope<TState>(TState state) => _scopeProvider.Push(state);

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception, string> formatter) {
			var sb = new StringBuilder();
			_scopeProvider.ForEachScope((scope, state) => {
				state.Append("\n => ");
				state.Append(scope);
			}, sb);
			sb.Append(state);
			_output.WriteLine(sb.ToString());
		}

		private static string GetLogLevelString(LogLevel logLevel) =>
			logLevel switch {
				LogLevel.Trace => "trce",
				LogLevel.Debug => "dbug",
				LogLevel.Information => "info",
				LogLevel.Warning => "warn",
				LogLevel.Error => "fail",
				LogLevel.Critical => "crit",
				_ => throw new ArgumentOutOfRangeException(nameof(logLevel))
			};
	}

	public class XunitLogger<T> : XunitLogger, ILogger<T> {
		public XunitLogger(IAsyncTextOutput output, LoggerExternalScopeProvider scopeProvider)
			: base(output, scopeProvider, typeof(T).FullName!) {
		}
	}
}
