using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathtone.Sdk.Logging.Tests {
	public class RelayLoggerTests {

		[Fact]
		public void TestLogMessage() {

			var output = new List<string>();
			var log = new RelayLogger<RelayLoggerTests>(new(), (_, _, _, msg) => output.Add(msg));

			using var s1 = log.BeginScope("TEST A");
			using var s2 = log.BeginScope("TEST B");
			log.LogInformation("TEST 1");
			Assert.True(log.IsEnabled(LogLevel.Information));
			Assert.Equal(new[] { "=> TEST A\r\n=> TEST B: TEST 1" }, output);
		}
	}
}