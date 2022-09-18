using Mathtone.Sdk.Utilities.Resources;
using System.Runtime.Serialization.Formatters.Binary;

namespace Mathtone.Sdk.Utilities.Tests {

	public class MaterializedLocalResourceTests {

		static readonly TestQueries1 Q1 = new();
		static readonly TestResources Q2 = new();


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
	[ResourcePath("RES")]
	public class TestQueries1 : MaterializedLocalResources {
		public readonly string Value1;
		[Resource("Value2")]
		public readonly string Value2;
		public string Value3 { get; init; }
		[Resource("ValueFour")]
		public string Value4 { get; init; }
	}

	public class TestResources : MaterializedLocalResources {
		public readonly string Value1;
		[Resource("Value2")]
		public readonly string Value2;
		public string Value3 { get; init; }
		[Resource("ValueFour")]
		public string Value4 { get; init; }
	}
#pragma warning restore CS8618
}