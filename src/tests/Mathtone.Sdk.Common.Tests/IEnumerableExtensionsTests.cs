using Mathtone.Sdk.Common.Extensions;
using System.Data;

namespace Mathtone.Sdk.Common.Tests {
	public class IEnumerableExtensionsTests {

		[Fact]
		public void ForEach() {

			var rslt = new List<int>();
			var items = new[] { 1, 2, 3, 4, 5 };
			items.ForEach(rslt.Add);
			Assert.Equal(items, rslt);
		}


		[Fact]
		public void For() {

			var expected = new[] { 1, 2, 3, 4, 5 };
			var i = 0;
			Assert.Equal(expected, Repeat.For(5, () => ++i).ToArray());
		}

		[Fact]
		public void While() {

			var expected = new[] { 1, 2, 3, 4, 5 };
			var i = 0;
			Assert.Equal(expected, Repeat.While(()=>i < 5, () => ++i).ToArray());
		}
	}
}