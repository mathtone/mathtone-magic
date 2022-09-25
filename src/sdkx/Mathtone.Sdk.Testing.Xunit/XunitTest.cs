using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Mathtone.Sdk.Testing.Xunit {
	
	public abstract class XunitTestBase<T> : XunitTestBase {
		protected XunitTestBase(ITestOutputHelper output) : base(output) {
		}
	}

	public abstract class XunitTestBase  {

		protected ITestOutputHelper Output { get; }

		protected XunitTestBase(ITestOutputHelper output) {
			Output = output;
		}
	}
}