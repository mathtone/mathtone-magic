using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using Xunit;
using Xunit.Abstractions;

namespace Mathtone.Sdk.Testing.Xunit {

	public abstract class XunitTestBase<T> : XunitTestBase, IClassFixture<T> where T : class{
		
		protected T Context { get; }

		protected XunitTestBase(ITestOutputHelper output, T context) : base(output) {
			Context = context;
		}
	}

	public abstract class XunitTestBase  {

		ILogger? _log;
		protected virtual ILogger Log => _log ??= CreateLogger();

		protected ITestOutputHelper Output { get; }

		protected XunitTestBase(ITestOutputHelper output) {
			Output = output;
		}

		protected virtual ILogger<T> CreateLogger<T>(string name = "") => new XunitLogger<T>(Output, new());
		protected virtual ILogger CreateLogger(string name = "") => new XunitLogger(Output, new(), name);
	}
}