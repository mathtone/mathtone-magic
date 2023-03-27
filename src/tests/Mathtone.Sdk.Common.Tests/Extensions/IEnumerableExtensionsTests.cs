using Mathtone.Sdk.Common.Extensions;

namespace Mathtone.Sdk.Common.Tests.Extensions {




	public class IEnumerableExtensionsTests {

		[Fact]
		public void ForEach() {

			var rslt = new List<int>();
			var items = new[] { 1, 2, 3, 4, 5 };
			items.ForEach(rslt.Add);
			Assert.Equal(items, rslt);
		}


		//[Fact]
		//public void For() {

		//	var expected = new[] { 1, 2, 3, 4, 5 };
		//	var i = 0;
		//	Assert.Equal(expected, Repeat.For(5, () => ++i).ToArray());
		//}

		//[Fact]
		//public void While_1() {

		//	var expected = new[] { 1, 2, 3, 4, 5 };
		//	var i = 0;
		//	Assert.Equal(expected, Repeat.While(() => i < 5, () => ++i).ToArray());
		//}

		//[Fact]
		//public void While_2() {

		//	var i = 10;
		//	Repeat.While(() => i > 0, () => { i--; });
		//	Assert.Equal(0, i);
		//}

		[Fact]
		public async Task ForEachAsync() {
			// Arrange
			var rslt = new List<int>();
			var items = new[] { 1, 2, 3, 4, 5 };

			// Act
			await items.ForEachAsync(async i => {
				await Task.Delay(10); // Introduce a small delay to simulate asynchronous processing
				rslt.Add(i);
			});

			// Assert
			Assert.Equal(items, rslt);
		}
	}
}