using Mathtone.Sdk.Testing.Xunit;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace _Sandbox {

	

	public class LoggingTests : ITestOutputHelper {

		List<string> _output = new();

		[Fact]
		public void MakeLogs() {
			var logger = new XunitLogger(this, new(), nameof(LoggingTests));
			logger.LogInformation("TEST");
			Assert.Equal(new[] { "TEST" }, _output);
		}

#pragma warning disable xUnit1013 // Public method should be marked as test
		public void WriteLine(string message) =>
			_output.Add(message);

		public void WriteLine(string format, params object[] args) =>
			WriteLine(string.Format(format, args));
#pragma warning restore xUnit1013
	}
}