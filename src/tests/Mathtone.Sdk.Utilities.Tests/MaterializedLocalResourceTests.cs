using Mathtone.Sdk.Testing.Xunit;
using Mathtone.Sdk.Utilities.Resources;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Mathtone.Sdk.Utilities.Tests {


	public class TextOutputAdapterTests : XunitTestBase {

		readonly List<string> _messages = new();

		public TextOutputAdapterTests(ITestOutputHelper output) :
			base(output) {
		}

		[Fact]
		public async Task WriteOutput_1() {
			var adapter = new TextOutputAdapter(WriteAsyncHandler) as IAsyncTextOutput;
			await adapter.WriteLineAsync("TEST1");
			adapter.WriteLine("TEST2");
			Assert.Equal(new[] { $"TEST1{Environment.NewLine}", $"TEST2{Environment.NewLine}" }, _messages);
		}
		[Fact]
		public async Task WriteOutput_2() {
			var adapter = new TextOutputAdapter(WriteHandler) as IAsyncTextOutput;
			await adapter.WriteLineAsync("TEST1");
			adapter.WriteLine("TEST2");
			Assert.Equal(new[] { $"TEST1{Environment.NewLine}", $"TEST2{Environment.NewLine}" }, _messages);
		}
		[Fact]
		public async Task WriteOutput_3() {
			var adapter = new TextOutputAdapter(WriteAsyncHandler, WriteHandler) as IAsyncTextOutput;
			await adapter.WriteLineAsync("TEST1");
			adapter.WriteLine("TEST2");
			Assert.Equal(new[] { $"TEST1{Environment.NewLine}", $"TEST2{Environment.NewLine}" }, _messages);
		}

		void WriteHandler(string message) {
			_messages.Add(message);
		}

		Task WriteAsyncHandler(string message) {
			WriteHandler(message);
			return Task.CompletedTask;
		}
	}

	public class MaterializedLocalResourceTests : XunitTestBase {

		static readonly TestResources1 Q1 = new();
		static readonly ResourceSchmeesource Q2 = new();

		public MaterializedLocalResourceTests(ITestOutputHelper output) :
			base(output) {
		}

		[Fact]
		public void LoadResources() {

			Assert.Equal("TEST VALUE 1", Q1.Value1);
			Assert.Equal("TEST VALUE 2", Q1.Value2);
			Assert.Equal("TEST VALUE 3", Q1.Value3);
			Assert.Equal("TEST VALUE 4", Q1.Value4);

			Assert.Equal("TEST VALUE 1", Q2.Value1);
			Assert.Equal("TEST VALUE 2", Q2.Value2);
			Assert.Equal("TEST VALUE 3", Q2.Value3);
			Assert.Equal("TEST VALUE 4", Q2.Value4);
		}
	}

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

	public class TestResources1 : MaterializedLocalResources {
		public readonly string Value1;
		[Resource("Value2")]
		public readonly string Value2;
		public string Value3 { get; init; }
		[Resource("ValueFour")]
		public string Value4 { get; init; }
	}

	[ResourcePath("TestResources2")]
	public class ResourceSchmeesource : MaterializedLocalResources {
		public readonly string Value1;
		[Resource("Value2")]
		public readonly string Value2;
		public string Value3 { get; init; }
		[Resource("ValueFour")]
		public string Value4 { get; init; }
	}

#pragma warning restore CS8618
}