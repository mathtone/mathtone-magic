using Microsoft.Extensions.Logging;

namespace Mathtone.Sdk.Services.Tests {
	public class AppServiceBaseTests {
		[Fact]
		public void AppServiceBase() {
			var svc = new AppTestService(null, new() { Name = "SomeTestService" });
			Assert.Equal("SomeTestService", svc.ServiceName);
		}

		[Fact]
		public void ServiceBase() {
			var svc = new SimpleTestService(null);
			Assert.Equal("SimpleTestService", svc.ServiceName);
		}
	}

	public class TestServiceConfig {
		public string Name { get; set; } = "";
	}

	public class SimpleTestService : AppServiceBase{
		public SimpleTestService(ILogger log) : base(log) { }
	}

	public class AppTestService : ConfiguredServiceBase<TestServiceConfig> {
		public AppTestService(ILogger log, TestServiceConfig config) : base(log, config) { }
		public override string ServiceName => Config.Name;
	}
}