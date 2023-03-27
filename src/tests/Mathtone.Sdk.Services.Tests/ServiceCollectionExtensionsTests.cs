using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathtone.Sdk.Services.Tests {
	interface ITestService {
		string Name { get; }
	}

	class TestService : ITestService {
		public string Name { get; }
		public TestService(string name) {
			Name = name;
		}
	}

	//Add xunit test class
	public class IServiceProviderExtensionsTests {

		[Fact]
		public void ActivateGeneric_ShouldReturnCorrectType() {
			//Arrange
			var services = new ServiceCollection().BuildServiceProvider();

			//Act
			var result = services.Activate<ITestService, TestService>("Hello!");

			//Assert
			Assert.Equal("Hello!", result.Name);
		}
	}

	public class ServiceCollectionExtensionsTests {

		readonly ServiceCollection _services = new();

		[Fact]
		public void AddActivator() {
			var svc = _services.AddActivator<TestService>(ServiceLifetime.Scoped, "TEST");
			Assert.Equal(ServiceLifetime.Scoped, svc.Single(d => d.ServiceType == typeof(TestService)).Lifetime);
			Assert.Equal("TEST", svc.BuildServiceProvider().GetRequiredService<TestService>().Name);
		}

		[Fact]
		public void ActivateSingleton() {
			var svc = _services.ActivateSingleton<TestService>("TEST");
			Assert.Equal(ServiceLifetime.Singleton, svc.Single(d => d.ServiceType == typeof(TestService)).Lifetime);
			Assert.Equal("TEST", svc.BuildServiceProvider().GetRequiredService<TestService>().Name);
		}


		[Fact]
		public void ActivateTransient() {
			var svc = _services.ActivateTransient<TestService>("TEST");
			Assert.Equal(ServiceLifetime.Transient, svc.Single(d => d.ServiceType == typeof(TestService)).Lifetime);
			Assert.Equal("TEST", svc.BuildServiceProvider().GetRequiredService<TestService>().Name);
		}

		[Fact]
		public void ActivateScoped() {
			var svc = _services.ActivateScoped<TestService>("TEST");
			Assert.Equal(ServiceLifetime.Scoped, svc.Single(d => d.ServiceType == typeof(TestService)).Lifetime);
			Assert.Equal("TEST", svc.BuildServiceProvider().GetRequiredService<TestService>().Name);
		}
	}
}