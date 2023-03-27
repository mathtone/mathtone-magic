using Mathtone.Sdk.Common.Extensions;

namespace Mathtone.Sdk.Common.Tests.Extensions {
	public class WaitHandleExtensionsTests {
		[Fact]
		public async Task WaitOneAsync_SetsTaskResultWhenWaitHandleIsSignaled() {
			// Arrange
			var waitHandle = new ManualResetEvent(initialState: false);

			// Act
			var task = waitHandle.WaitOneAsync();
			waitHandle.Set();
			await task;

			// Assert
			Assert.True(task.IsCompletedSuccessfully);
		}




		[Fact]
		public async Task WaitOneAsync_UnregistersWaitHandleAfterCompletion() {
			// Arrange
			var waitHandle = new ManualResetEvent(initialState: false);

			// Act
			var task = waitHandle.WaitOneAsync();
			waitHandle.Set();
			await task;

			// Assert
			Assert.True(task.IsCompletedSuccessfully);
			await Task.Delay(100); // Give some time for the unregister to execute
			Assert.True(task.IsCompletedSuccessfully);
		}

		[Fact]
		public async Task WaitOneAsync_ThrowsArgumentNullExceptionWhenWaitHandleIsNull() {
			// Arrange
			WaitHandle waitHandle = null;

			// Act and assert
			await Assert.ThrowsAsync<ArgumentNullException>(() => waitHandle.WaitOneAsync());
		}
	}
}