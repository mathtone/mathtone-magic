using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System.Reflection;
using System.Text;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Mathtone.Sdk.Testing.Xunit.Tests {

	public class XunitLoggerTests : XunitTestBase {

		readonly StringBuilder sb = new();

		public XunitLoggerTests(ITestOutputHelper output) :
			base(output) {
		}

		[Fact]
		public void LogInformation() {
			CreateLog().LogInformation("TEST");
			Assert.Equal($"TEST{Environment.NewLine}", sb.ToString());
		}

		//[Fact]
		//public void LogInformation() {
		//	CreateLog().LogInformation("TEST");
		//	Assert.Equal($"TEST{Environment.NewLine}", sb.ToString());
		//}

		public override void Write(string text) {
			sb.Append(text);
			base.Write(text);
		}

		public override Task WriteAsync(string text) {
			sb.Append(text);
			return base.WriteAsync(text);
		}
	}
}