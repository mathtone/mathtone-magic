using Mathtone.Sdk.Common.Extensions;
using System.Data;

namespace Mathtone.Sdk.Common.Tests.Extensions {
	public class RepeatTests {

		[Fact]
		public void For_ReturnsExpectedValues() {
			// Arrange
			var expected = new[] { 1, 2, 3, 4, 5 };
			var i = 0;

			// Act
			var actual = Repeat.For(5, () => ++i).ToArray();

			// Assert
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void While_ReturnsExpectedValues() {
			// Arrange
			var expected = new[] { 1, 2, 3, 4, 5 };
			var i = 0;

			// Act
			var actual = Repeat.While(() => i < 5, () => ++i).ToArray();

			// Assert
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void While_WithAction_CallsAction() {
			// Arrange
			var count = 0;

			// Act
			Repeat.While(() => count < 5, () => count++).ToArray();

			// Assert
			Assert.Equal(5, count);
		}

		[Fact]
		public void While_Action_CallsActionWhileConditionIsTrue() {
			// Arrange
			var count = 5;
			//var expected = count;

			// Act
			Repeat.While(() => count > 0,
				() => { count--; }
			);

			// Assert
			Assert.Equal(0, count);
		}

		[Fact]
		public async Task AwaitWhile_WithFunc_CallsAction() {
			// Arrange
			var count = 0;

			// Act
			await Repeat.AwaitWhile(() => count < 5, async () => { await Task.Delay(1); count++; });

			// Assert
			Assert.Equal(5, count);
		}

		[Fact]
		public async Task AwaitWhile_WithTaskFunc_CallsAction() {
			// Arrange
			var count = 0;

			// Act
			await Repeat.AwaitWhile(async () => await Task.FromResult(count < 5), async () => {
				await Task.Delay(1);
				count++;
			});

			// Assert
			Assert.Equal(5, count);
		}
	}

}