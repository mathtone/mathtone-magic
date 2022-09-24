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

//namespace _Sandbox {

//	public class LoggingTests : ITestOutputHelper {

//		readonly StringBuilder _sb = new();
//		readonly ITestOutputHelper _output;

//		public LoggingTests(ITestOutputHelper output) =>
//			_output = output;

//		[Fact]
//		public void MakeLogs() {

//			var logger = new XunitLogger(this, new(), nameof(LoggingTests));
//			logger.LogInformation("TEST 0");
//			using var s1 = logger.BeginScope("Scope1");
//			using var s2 = logger.BeginScope("Scope2");

//			logger.LogInformation("TEST A");
//			logger.LogInformation("TEST B");
//			Assert.Equal($": TEST 0{NewLine}=> Scope1\r\n=> Scope2: TEST A\r\n=> Scope1\r\n=> Scope2: TEST B\r\n", _sb.ToString());
//		}

//#pragma warning disable xUnit1013 // Public method should be marked as test
//		public void WriteLine(string message) {
//			_output.WriteLine(message);
//			_sb.AppendLine(message);
//		}

//		public void WriteLine(string format, params object[] args) => WriteLine(string.Format(format, args));
//#pragma warning restore xUnit1013
//	}
//}

/*
	1-800-974-6466 - disability?
	1-855-271-7773
	1-866-837-2719 soc. sec. presque isle
	1-unemployment
	532-5000 Maine care
*/