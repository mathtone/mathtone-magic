using Xunit;

namespace Mathtone.Sdk.Patterns.Tests {
	public class BroadcasterTests {

		[Fact]
		public async Task NoBroadcast_ReturnEmpty() {
			var broadcaster = new Broadcaster<int>();
			var subscriber1 = broadcaster.Subscribe();
			await broadcaster.DisposeAsync();
			Assert.Empty(await subscriber1.ReadAllAsync().ToArrayAsync());
			
		}

		[Fact]
		public async Task Subscribe_ShouldReturnLastValue() {
			var broadcaster = new Broadcaster<int>();
			await broadcaster.Send(1);
			var subscriber1 = broadcaster.Subscribe();

			Assert.Equal(1, broadcaster.Last);
			await broadcaster.DisposeAsync();
			
			Assert.Equal(new[] { 1 },await subscriber1.ReadAllAsync().ToArrayAsync());

		}
		[Fact]
		public async Task Send_ShouldSendToSubscribers() {
			// Arrange
			var broadcaster = new Broadcaster<int>();
			var subscriber1 = broadcaster.Subscribe();
			var subscriber2 = broadcaster.Subscribe();
			var subscriber3 = broadcaster.Subscribe();

			// Act
			await broadcaster.Send(1);
			await broadcaster.Send(2);
			await broadcaster.Send(3);

			await broadcaster.DisposeAsync();
			var expected = new[] { 1, 2, 3 };
			Assert.Equal(expected, await subscriber1.ReadAllAsync().ToArrayAsync());
			Assert.Equal(expected, await subscriber2.ReadAllAsync().ToArrayAsync());
			Assert.Equal(expected, await subscriber3.ReadAllAsync().ToArrayAsync());
		}

		[Fact]
		public async Task Dispose_ShouldCloseSubscribers() {
			// Arrange
			var broadcaster = new Broadcaster<int>();
			var subscriber = broadcaster.Subscribe();
			// Act
			await broadcaster.DisposeAsync();
			// Assert
			Assert.Empty(await subscriber.ReadAllAsync().ToArrayAsync());

		}
	}
}