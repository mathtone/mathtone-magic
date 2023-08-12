using Microsoft.Extensions.DependencyInjection;

namespace Mathtone.Sdk.Services.Tests {
	public class IServiceProviderExtensionsTests {

		[Fact]
		public void ActivateGeneric_ShouldReturnCorrectType() {
			var services = new ServiceCollection().BuildServiceProvider();
			var result = services.Activate<ITestService, TestService>("Hello!");
			Assert.Equal("Hello!", result.Name);
		}
	}
}