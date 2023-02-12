using Mathtone.Sdk.Services;
using Mathtone.Sdk.Testing.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using Xunit.Abstractions;

namespace Mathtone.Sdk.BuildUtilities.Tests {
	public class UnitTest1 : XunitServiceTestBase {

		public UnitTest1(ITestOutputHelper output) :
			base(output) { }

		[Fact]
		public void Test1() {
			var svc1 = Services.Activated<ITestActivationTarget,string>("TESTVALUE");
			var svc2 = Services.Activated<ITestActivationTarget>("TESTVALUE");
			;
		}

		protected override IServiceCollection OnConfigureServices(IServiceCollection services) => base
			.OnConfigureServices(services)
			.AddActivation<ITestActivationTarget, TestActivationTarget>()
			.AddActivation<ITestActivationTarget, TestActivationTarget,string>()
			.AddSingleton<SolutionProcessor>();
	}
	public interface ITestActivationTarget { }
	public class TestActivationTarget : ITestActivationTarget {
		public TestActivationTarget(string input) {
			;
		}
	}

	public class SolutionProcessor : LoggedServiceBase {
		public SolutionProcessor(ILogger<SolutionProcessor> logger) :
			base(logger) { }
	}

	public static class IServiceCollectionExtensions {

	}
}