﻿using Mathtone.Sdk.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Mathtone.Sdk.Testing.Xunit {
	[Obsolete("Use XunitServiceTestBase instead", true)]
	public class XunitLogger : Logger {

		readonly ITestOutputHelper _output;

		public XunitLogger(ITestOutputHelper output, LoggerExternalScopeProvider scopeProvider, string categoryName) :
			base(scopeProvider, categoryName) {
			_output = output;
		}

		public override bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

		protected override void OnWrite(LogLevel level, EventId eventId, Exception? exception, string message) {
			_output.WriteLine(message);
		}
	}

	[Obsolete("Use XunitServiceTestBase & XunitServiceTestLogger instead", true)]
	public class XunitLogger<T> : XunitLogger, ILogger<T> {
		public XunitLogger(ITestOutputHelper output, LoggerExternalScopeProvider scopeProvider)
			: base(output, scopeProvider, typeof(T).FullName!) {
		}
	}


	[Obsolete("Use XunitServiceTestBase & XunitServiceTestLogger instead", true)]
	public static class ServiceCollectionExtensions {
		public static IServiceCollection AddXunitLogging(this IServiceCollection services, ITestOutputHelper output) => services
			.AddSingleton(output)
			.AddSingleton<LoggerExternalScopeProvider>()
			.AddTransient(typeof(ILogger<>), typeof(XunitLogger<>));
	}
}