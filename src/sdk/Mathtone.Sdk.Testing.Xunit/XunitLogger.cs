using Mathtone.Sdk.Common.Utility;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Mathtone.Sdk.Testing.Xunit {

	public abstract class XunitTest : IAsyncTextOutput {

		protected XunitOutputAdapter Output { get; }

		protected XunitTest(ITestOutputHelper output) {
			Output = new(output);
		}

		public virtual Task WriteAsync(string text) =>
			Output.WriteAsync(text);

		public virtual void Write(string text) =>
			Output.Write(text);

		protected ILogger CreateLog() => XunitLogger.Create(this);
		protected ILogger<T> CreateLog<T>() => XunitLogger.Create<T>(this);
	}

	public class XunitLogger : ILogger {

		private readonly IAsyncTextOutput _output;
		private readonly string _categoryName;
		private readonly LoggerExternalScopeProvider _scopeProvider;

		public XunitLogger(IAsyncTextOutput output, LoggerExternalScopeProvider scopeProvider, string categoryName) {
			_output = output;
			_scopeProvider = scopeProvider;
			_categoryName = categoryName;
		}

		public static ILogger Create(IAsyncTextOutput output) =>
			new XunitLogger(output, new LoggerExternalScopeProvider(), "");

		public static ILogger<T> Create<T>(IAsyncTextOutput output) =>
			new XunitLogger<T>(output, new LoggerExternalScopeProvider());

		public bool IsEnabled(LogLevel logLevel) =>
			logLevel != LogLevel.None;

		public IDisposable BeginScope<TState>(TState state) =>
			_scopeProvider.Push(state);

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) {
			var sb = new StringBuilder();
			_scopeProvider.ForEachScope((scope, state) => {
				state.Append("\n => ");
				state.Append(scope);
			}, sb);
			sb.Append(formatter(state,exception));
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