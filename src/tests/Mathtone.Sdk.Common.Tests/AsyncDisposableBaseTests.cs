namespace Mathtone.Sdk.Common.Tests {
	public class AsyncDisposableBaseTests {

		[Fact]
		public async Task DisposeAsync_CallsOnDisposeAsync() {
			// Arrange
			var disposable = new TestAsyncDisposable();

			// Act
			await disposable.DisposeAsync();

			// Assert
			Assert.True(disposable.OnDisposeAsyncCalled);
		}

		
		private class TestAsyncDisposable : AsyncDisposableBase {
			public bool OnDisposeAsyncCalled { get; private set; }
			public bool SuppressFinalizeCalled { get; private set; }

			protected override async ValueTask OnDisposeAsync() {
				await base.OnDisposeAsync();
				OnDisposeAsyncCalled = true;
				await Task.Delay(1);
			}

			~TestAsyncDisposable() {
				SuppressFinalizeCalled = true;
			}
		}
	}

}