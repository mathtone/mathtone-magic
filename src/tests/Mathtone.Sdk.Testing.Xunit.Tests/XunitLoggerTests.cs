using Mathtone.Sdk.Common.Utility;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System.Reflection;
using System.Text;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Mathtone.Sdk.Testing.Xunit.Tests {

	public class XunitLoggerTests : IAsyncTextOutput {

		readonly XunitOutputAdapter _output;
		readonly StringBuilder sb = new();

		public XunitLoggerTests(ITestOutputHelper output) {
			_output = new(output);
		}

		[Fact]
		public void LogInformation() {
			XunitLogger.Create<XunitLoggerTests>(this).LogInformation("TEST");
			Assert.Equal($"TEST{Environment.NewLine}", sb.ToString());
		}

		Task IAsyncTextOutput.WriteAsync(string text) {
			sb.Append(text);
			return _output.WriteAsync(text);
		}

		void ITextOutput.Write(string text) {
			sb.Append(text);
			_output.Write(text);
		}
	}
}