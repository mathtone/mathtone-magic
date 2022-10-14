using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Newtonsoft.Json.Bson;
using System.Reflection;
using System.Text;
using Xunit.Abstractions;
using Xunit.Sdk;
using static System.Environment;
namespace Mathtone.Sdk.Testing.Xunit.Tests {

	public class XUnitTestTestsContext {
	}

	public class XunitTestTests : XunitTestBase<XUnitTestTestsContext> {

		public XunitTestTests(ITestOutputHelper output, XUnitTestTestsContext context) : base(output, context) { }

		[Fact]
		public void OutputTest() {
			Assert.NotNull(Output);
			Output.WriteLine("WRITTEN");
		}

		[Fact]
		public void ContextTest() => Assert.NotNull(Context);
	}

	public class XunitLoggerTests : XunitTestBase, ITestOutputHelper {

		readonly StringBuilder _sb = new();

		public XunitLoggerTests(ITestOutputHelper output) :
			base(output) { }

		[Fact]
		public void LogOutput() {

			var log = new XunitLogger<XunitLoggerTests>(this, new());
			Assert.True(log.IsEnabled(LogLevel.Information));
			log.LogInformation("TEST");
			Assert.Equal($": TEST{NewLine}", _sb.ToString());
		}

#pragma warning disable xUnit1013 // Public method should be marked as test
		public void WriteLine(string message) {
			_sb.AppendLine(message);
			Output.WriteLine(message);
		}

		public void WriteLine(string format, params object[] args) =>
			WriteLine(string.Format(format, args));
#pragma warning restore xUnit1013 // Public method should be marked as test

	}
}