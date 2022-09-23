using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System.Reflection;
using System.Text;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Mathtone.Sdk.Testing.Xunit.Tests {

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