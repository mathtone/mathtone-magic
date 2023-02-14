using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Reflection;
using Xunit.Abstractions;
using Mathtone.Sdk.Services;

namespace Mathtone.Sdk.Testing.Xunit.Tests {

	public class XunitTestLoggerTests : XunitServiceTestBase {

		public XunitTestLoggerTests(ITestOutputHelper output) :
			base(output) { }

		[Fact]
		public void CreateLogger() {
			Log.LogInformation("TEST");
			Assert.True(true);
		}
	}
}