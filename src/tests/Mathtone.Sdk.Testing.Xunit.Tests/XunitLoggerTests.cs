using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Newtonsoft.Json.Bson;
using System.Reflection;
using System.Text;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Mathtone.Sdk.Testing.Xunit.Tests {

	public class XUnitTestTestsContext {
	}

	public class XunitTestTests : XunitTestBase<XUnitTestTestsContext> {

		public XunitTestTests(ITestOutputHelper output) : base(output) { }

		[Fact]
		public void OutputTest() {
			Assert.NotNull(Output);
			Output.WriteLine("WRITTEN");
		}
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
			Assert.Equal(": TEST\r\n", _sb.ToString());
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
	//public class XunitLoggerTests : XunitTestBase {

	//	readonly StringBuilder sb = new();

	//	public XunitLoggerTests(ITestOutputHelper output) :
	//		base(output) {
	//	}

	//	[Fact]
	//	public void CreateTypedLog() {
	//		var log = CreateLog<XunitLogger>();
	//		Assert.True(log.IsEnabled(LogLevel.Information));
	//		using (log.BeginScope("Scoped")) {
	//			log.LogInformation("TEST");
	//		}


	//		Assert.Equal($"TEST{Environment.NewLine}", sb.ToString());
	//	}

	//	[Fact]
	//	public void LogInformation() {
	//		CreateLog().LogInformation("TEST");
	//		Assert.Equal($"TEST{Environment.NewLine}", sb.ToString());
	//	}

	//	[Fact]
	//	public async Task WriteAsyncMessage() {
	//		await this.WriteAsync("TEST");
	//		Assert.Equal($"TEST", sb.ToString());
	//	}

	//	//[Fact]
	//	//public void LogInformation() {
	//	//	CreateLog().LogInformation("TEST");
	//	//	Assert.Equal($"TEST{Environment.NewLine}", sb.ToString());
	//	//}

	//	public override void Write(string text) {
	//		sb.Append(text);
	//		base.Write(text);
	//	}

	//	public override Task WriteAsync(string text) {
	//		sb.Append(text);
	//		return base.WriteAsync(text);
	//	}
	//}
}