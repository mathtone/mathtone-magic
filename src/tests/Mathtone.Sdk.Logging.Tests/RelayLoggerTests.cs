using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Environment;


namespace Mathtone.Sdk.Logging.Tests {

	public class TestLogger : Logger {
		
		public TestLogger() : base() { }

		public override bool IsEnabled(LogLevel logLevel) {
			throw new NotImplementedException();
		}

		protected override void OnWrite(LogLevel level, EventId eventId, Exception? exception, string message) {
			throw new NotImplementedException();
		}
	}
	public class LoggerTests {
		[Fact]
		public void TestLogger() {
			Assert.NotNull(new TestLogger());
		}
	}
	public class RelayLoggerTests {

		[Fact]
		public void TestLogMessage() {

			var output = new List<string>();
			var log = new RelayLogger<RelayLoggerTests>(new(), (_, _, _, msg) => output.Add(msg));

			using var s1 = log.BeginScope("TEST A");
			using var s2 = log.BeginScope("TEST B");
			log.LogInformation("TEST 1");
			Assert.True(log.IsEnabled(LogLevel.Information));
			Assert.Equal(new[] { $"=> TEST A{NewLine}=> TEST B: TEST 1" }, output);
		}
	}
}