using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Configuration;

namespace _Sandbox.Logging.Console {
	public static class ConsoleLoggerExtensions {

		public static ILoggingBuilder AddConsoleLogger(
			this ILoggingBuilder builder) {
			builder.AddConfiguration();

			builder.Services.TryAddEnumerable(
				ServiceDescriptor.Singleton<ILoggerProvider, ConsoleLoggerProvider>());

			LoggerProviderOptions.RegisterProviderOptions
				<ConsoleLoggerConfig, ConsoleLoggerProvider>(builder.Services);

			return builder;
		}
	}
}