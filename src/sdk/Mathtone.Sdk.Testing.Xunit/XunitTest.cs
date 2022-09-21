using Mathtone.Sdk.Common.Utility;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Mathtone.Sdk.Testing.Xunit {
	public abstract class XunitTest : IAsyncTextOutput {

		protected XunitOutputAdapter Output { get; }

		protected XunitTest(ITestOutputHelper output) {
			Output = new(output);
		}

		public virtual Task WriteAsync(string text) =>
			Output.WriteAsync(text);

		public virtual void Write(string text) =>
			Output.Write(text);

		protected ILogger CreateLog() => XunitLogger.Create(this);
		protected ILogger<T> CreateLog<T>() => XunitLogger.Create<T>(this);
	}
}