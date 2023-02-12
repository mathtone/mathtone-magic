using Mathtone.Sdk.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Mathtone.Sdk.Testing.Xunit {

	public abstract class XunitServiceTestBase {

		protected XunitServiceTestBase(ITestOutputHelper output) {
			Output = output;
			var svc = new ServiceCollection();
			Services = OnConfigureServices(svc).BuildServiceProvider();
			Log = (ILogger)Services.GetRequiredService(typeof(ILogger<>).MakeGenericType(GetType()));
		}

		protected readonly ILogger Log;
		protected readonly IServiceProvider Services;
		protected readonly ITestOutputHelper Output;

		protected virtual T GetService<T>() where T : notnull => Services.GetRequiredService<T>();
		protected virtual IEnumerable<T> GetServices<T>() => Services.GetServices<T>();
		protected virtual T ActivateServices<T>(params object[] args) => Services.Activate<T>(args);

		protected virtual IServiceCollection OnConfigureServices(IServiceCollection services) => services
			.AddLogging(bld => bld
				.AddXunitTestLogger(Output)
				.AddConsole()
			);
	}

	public abstract class XunitServiceTestBase<CTX> : XunitServiceTestBase, IClassFixture<CTX> where CTX : class {
		protected XunitServiceTestBase(ITestOutputHelper output, CTX context) :
			base(output) {
			Context = context;
		}

		protected CTX Context { get; }
	}
}