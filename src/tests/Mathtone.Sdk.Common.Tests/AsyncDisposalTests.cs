namespace Mathtone.Sdk.Common.Tests {
	public class AsyncDisposalTests {

		[Fact]
		public async Task Of_DisposesAllDisposables() {
			// Arrange
			var disposable1 = new TestAsyncDisposable();
			var disposable2 = new TestAsyncDisposable();

			// Act
			await AsyncDisposal.Of(disposable1, disposable2);

			// Assert
			Assert.True(disposable1.OnDisposeAsyncCalled);
			Assert.True(disposable2.OnDisposeAsyncCalled);
		}

		private class TestAsyncDisposable : IAsyncDisposable {
			public bool OnDisposeAsyncCalled { get; private set; }

			public async ValueTask DisposeAsync() {
				OnDisposeAsyncCalled = true;
				await Task.Delay(1);
			}
		}
	}

}