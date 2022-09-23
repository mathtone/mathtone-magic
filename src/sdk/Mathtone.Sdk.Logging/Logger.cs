using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathtone.Sdk.Logging {

	public delegate void LogHandler(LogLevel level, EventId eventId, Exception? exception, string message);

	public abstract class Logger : ILogger {

		private readonly string _category;
		private readonly LoggerExternalScopeProvider _scope;

		protected Logger(LoggerExternalScopeProvider scopeProvider, string categoryName) {
			_scope = scopeProvider;
			_category = categoryName;
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) =>
			OnWrite(logLevel, eventId, exception, formatter(state, exception));

		public abstract bool IsEnabled(LogLevel logLevel);

		public IDisposable BeginScope<TState>(TState state) => _scope.Push(state);

		protected abstract void OnWrite(LogLevel level, EventId eventId, Exception? exception, string message);
	}
}