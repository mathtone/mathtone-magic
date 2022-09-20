using Mathtone.Sdk.Common.Extensions;

namespace Mathtone.Sdk.Common.Tests {
	public class IEnumerableExtensionsTests {

		[Fact]
		public void ForEach() {

			var rslt = new List<int>();
			var items = new[] { 1, 2, 3, 4, 5 };
			items.ForEach(rslt.Add);
			Assert.Equal(items, rslt);
		}
	}
}