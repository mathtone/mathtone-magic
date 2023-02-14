using Mathtone.Sdk.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Xunit.Abstractions;

namespace Mathtone.Sdk.Testing.Xunit {
	public static class XunitTestLoggerExtensions {

		public static ILoggingBuilder AddXunitTestLogger(this ILoggingBuilder builder, ITestOutputHelper output) {
			builder.AddConfiguration();
			builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, XunitTestLoggerProvider>(svc => svc.Activate<XunitTestLoggerProvider>(output)));
			LoggerProviderOptions.RegisterProviderOptions<XunitTestLoggerLoggerConfiguration, XunitTestLoggerProvider>(builder.Services);
			return builder;
		}

		public static ILoggingBuilder AddColorConsoleLogger(this ILoggingBuilder builder, Action<XunitTestLoggerLoggerConfiguration> configure, ITestOutputHelper output) {
			builder.AddXunitTestLogger(output);
			builder.Services.Configure(configure);
			return builder;
		}
	}
}