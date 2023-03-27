namespace Mathtone.Sdk.Patterns.Tests {
	//Add xunit test class
	public class SubscriberTests {
		[Fact]
		public async Task SendAsync_ShouldSendItem() {
			// Arrange
			var subscriber = new Subscriber<int>();
			var item = 5;

			// Act
			await subscriber.SendAsync(item);

			// Assert
			var result = await subscriber.ReadAsync();
			Assert.Equal(item, result);
		}

		[Fact]
		public async Task ReadAllAsync_ShouldReturnAllItems() {
			// Arrange
			var subscriber = new Subscriber<int>();
			var items = new[] { 1, 2, 3 };

			// Act
			foreach (var item in items) {
				await subscriber.SendAsync(item);
			}
			subscriber.Close();
			// Assert
			var result = await subscriber.ReadAllAsync().ToListAsync();
			Assert.Equal(items, result);
		}

		[Fact]
		public void Close_ShouldRaiseClosingEvent() {
			// Arrange
			using var subscriber = new Subscriber<int>();
			var raised = false;
			subscriber.Closing += (sender, args) => raised = true;

			// Act
			subscriber.Close();

			// Assert
			Assert.True(raised);
		}
	}
}