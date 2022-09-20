using Mathtone.Sdk.Common.Utility;
using Mathtone.Sdk.Logging.Tests;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System.Reflection;
using System.Text;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Mathtone.Sdk.Testing.Xunit.Tests {

	public class UnitTest1 {


		XunitOutputAdapter _output;

		public UnitTest1(ITestOutputHelper output) {
			_output = new(output);
		}

		[Fact]
		public async Task Test1() {
			//var logger = XunitLogger.Create<UnitTest1>(_output);
			//logger.LogInformation("TEST");
		}
	}

	public class XunitOutputAdapter : TextOutputAdapter {
		public XunitOutputAdapter(ITestOutputHelper output) :
			base(output.WriteLine) {
		}
	}

	//public abstract class XunitTestBase : ITestOutputHelper {

	//	ITestOutputHelper _output;
	//	ILogger? _logger;
	//	protected ILogger Log => _logger ??= CreateLogger();

	//	protected XunitTestBase(ITestOutputHelper output) {
	//		_output = output;
	//	}

	//	ILogger CreateLogger() {
	//		var t = this.GetType();
	//		var mi = typeof(XunitLogger).GetMethod(
	//			"Create",
	//			1,
	//			new[] { t }
	//		);
	//		var meth = mi!.MakeGenericMethod(new[] { t });
	//		return (ILogger)meth.Invoke(null, new[] { this })!;
	//	}

	//	public virtual void WriteLine(string message) =>
	//		_output.WriteLine(message);

	//	public virtual void WriteLine(string format, params object[] args) =>
	//		_output.WriteLine(format, args);
	//}

	//public class XunitTestTest : XunitTestBase {

	//	StringBuilder _output = new();

	//	public XunitTestTest(ITestOutputHelper output) : base(output) {
	//	}

	//	[Fact]
	//	public void Test() {
	//		//using (Log.BeginScope("LOG OPEN")) ;
	//		Log.LogInformation("TEST");
	//		Assert.Equal($"TEST{Environment.NewLine}", _output.ToString());
	//	}


	//	public override void WriteLine(string message) {
	//		base.WriteLine(message);
	//		_output.AppendLine(message);

	//	}
	//}
}