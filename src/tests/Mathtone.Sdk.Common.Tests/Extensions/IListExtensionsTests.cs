using Mathtone.Sdk.Common.Extensions;

namespace Mathtone.Sdk.Common.Tests.Extensions {
	public class IListExtensionsTests {
		[Fact]
		public void Swap_SwapsTwoItemsInList() {
			// Arrange
			var list = new List<int> { 1, 2, 3 };

			// Act
			list.Swap(0, 2);

			// Assert
			Assert.Equal(3, list[0]);
			Assert.Equal(1, list[2]);
		}

		[Fact]
		public void Swap_ThrowsExceptionIfIndexesOutOfRange() {
			// Arrange
			var list = new List<int> { 1, 2, 3 };

			// Act & Assert
			Assert.Throws<ArgumentOutOfRangeException>(() => list.Swap(-1, 0));
			Assert.Throws<ArgumentOutOfRangeException>(() => list.Swap(0, 4));
		}
	}
}