using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathtone.Sdk.Services.Tests {

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

		[Fact]
		public void AddConfiguration() {
			var cfg = new ServiceCollection()
				.BuildConfigFromFile("appsettings.json")
				.AddConfiguration<TestConfiguration>()
				.BuildServiceProvider()
				.GetRequiredService<TestConfiguration>();
			Assert.Equal("TEST", cfg.Value);
		}


		[Fact]
		public void AddConfiguration_2() {
			var cfg = new ServiceCollection()
				.BuildConfigFromFile("appsettings.json")
				.AddConfiguration<TestConfiguration>("TestConfigurationSection")
				.BuildServiceProvider()
				.GetRequiredService<TestConfiguration>();
			Assert.Equal("SECTION", cfg.Value);
		}

		[Fact]
		public void AddConfiguration_3() {

			var cf = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.Build();

			var cfg = new ServiceCollection()
				.AddConfiguration<TestConfiguration>(cf)
				.BuildServiceProvider()
				.GetRequiredService<TestConfiguration>();

			Assert.Equal("TEST", cfg.Value);
		}
	}

	class TestConfiguration {
		public string? Value { get; set; }
	}

	interface ITestService {
		string Name { get; }
	}

	class TestService : ITestService {
		public string Name { get; }
		public TestService(string name) {
			Name = name;
		}
	}
}