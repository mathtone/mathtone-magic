namespace Mathtone.Sdk.Patterns.Tests {
	using Microsoft.Extensions.FileSystemGlobbing;
	using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
	using Microsoft.VisualStudio.TestPlatform.Utilities;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Threading.Channels;
	using System.Threading.Tasks;
	using Xunit;
	using Xunit.Abstractions;

	public class AsyncProcessorTests {

		private readonly ITestOutputHelper _output;

		public AsyncProcessorTests(ITestOutputHelper output) {
			_output = output;
		}

		[Fact]
		public async Task Get_Batches_From__Class() {

			var processor = new AsyncProcessor<int>();
			var processedItems = new List<int>();

			await processor.ProcessAsync(GetValues(), async batch => {
				processedItems.AddRange(batch);
				await Task.Delay(100);
				_output.WriteLine($"Processed {batch.Count()} items");
			}, 2000, TimeSpan.FromMilliseconds(500));

			Assert.Equal(1000, processedItems.Count);
		}

		[Theory]
		[InlineData(100, 10, 100, 10)]
		[InlineData(101, 10, 100, 11)]
		[InlineData(10, 3, 100, 4)]
		[InlineData(10, 20, 100, 1)]
		public async Task Get_Batches_From_Method(int values, long batchSize, int interval, long expectedBatches) {
			var processValues = Enumerable.Range(1, values).ToAsyncEnumerable();
			var batches = await AsyncBatches.GetBatches(processValues, batchSize, TimeSpan.FromMilliseconds(interval)).ToArrayAsync();
			var totalItems = batches.Sum(b => b.Count());

			Assert.Equal(values, totalItems);
			Assert.Equal(expectedBatches, batches.Count());

		}

		[Theory]
		[InlineData(100, 10, 100, 10)]
		[InlineData(101, 10, 100, 11)]
		[InlineData(10, 3, 100, 4)]
		[InlineData(10, 20, 100, 1)]
		public async Task Get_Batches_From_Class(int values, long batchSize, int interval, long expectedBatches) {
			
			var processValues = Enumerable.Range(1, values).ToAsyncEnumerable();
			var processor = new AsyncProcessor<int>();
			var processedItems = new List<int>();

			await processor.ProcessAsync(GetValues(), async batch => {
				processedItems.AddRange(batch);
				await Task.Delay(100);
				_output.WriteLine($"Processed {batch.Count()} items");
			}, batchSize, TimeSpan.FromMilliseconds(interval));

			//Assert.Equal(values, totalItems);
			Assert.Equal(expectedBatches, batches.Count());

		}

		[Fact]
		public async Task Get_Batches_From_Method_With_Interval() {
			var batches = await AsyncBatches.GetBatches(GetValues2(), 100, TimeSpan.FromMilliseconds(10)).Cast<IList<int>>().ToArrayAsync();
			var totalItems = batches.Sum(b => b?.Count ?? 0);

			Assert.Equal(20, totalItems);
			Assert.Equal(1, batches.Count());

			;
			//Assert.Equal(10, batches[1]!.Length);
		}

		static async IAsyncEnumerable<int> GetValues2() {
			await foreach (var i in Enumerable.Range(1, 10).ToAsyncEnumerable()) {
				yield return i;
			}
			await Task.Delay(1000);
			await foreach (var i in Enumerable.Range(11, 10).ToAsyncEnumerable()) {
				yield return i;
			}
		}

		static async IAsyncEnumerable<int> GetValues() {
			await foreach (var i in Enumerable.Range(1, 499).ToAsyncEnumerable()) {
				yield return i;
			}
			await Task.Delay(1000);
			await foreach (var i in Enumerable.Range(500, 501).ToAsyncEnumerable()) {
				yield return i;
			}
		}
	}

	public static class AsyncBatches {
		public static async IAsyncEnumerable<IEnumerable<T>> GetBatches<T>(IAsyncEnumerable<T> input, long batchSize, TimeSpan interval) {
			var buffer = new List<T>();
			var locker = new object();
			var semaphore = new SemaphoreSlim(0, 1);
			var cancellationTokenSource = new CancellationTokenSource();

			using var timer = new Timer(_ =>
			{
				lock (locker) {
					if (buffer.Any()) {
						cancellationTokenSource.Cancel();
					}
				}
			}, null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan); // Initially stopped

			await foreach (var item in input) {
				lock (locker) {
					buffer.Add(item);
					if (buffer.Count == 1) {
						// Start the timer when the first item is added
						timer.Change(interval, Timeout.InfiniteTimeSpan);
					}
				}

				if (buffer.Count >= batchSize) {
					cancellationTokenSource.Cancel();
				}
				else {
					try {
						// Try to wait for the timer's callback or until the batch size is reached
						await semaphore.WaitAsync(interval, cancellationTokenSource.Token);
					}
					catch (OperationCanceledException) {
						// Cancellation is expected in this design
					}
				}

				List<T> currentBatch;
				lock (locker) {
					currentBatch = buffer.ToList();
					buffer.Clear();
					timer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan); // Stop the timer after yielding
				}
				if (currentBatch.Any()) {
					yield return currentBatch;
				}

				// Reset the cancellation token for the next iteration
				cancellationTokenSource.Dispose();
				cancellationTokenSource = new CancellationTokenSource();
			}

			timer.Dispose();

			if (buffer.Any()) {
				yield return buffer; // Yield any remaining items
			}
		}
	}

	public static class AsyncBatchesX {
		public static async IAsyncEnumerable<IEnumerable<T>> GetBatches<T>(IAsyncEnumerable<T> input, long batchSize, TimeSpan interval) {
			var buffer = new List<T>();
			var batchReady = new TaskCompletionSource<bool>();
			var lastYieldTime = DateTime.UtcNow;
			var locker = new object();

			var timer = new Timer(_ => {
				lock (locker) {
					if (DateTime.UtcNow - lastYieldTime >= interval && buffer.Any() && !batchReady.Task.IsCompleted) {
						batchReady.TrySetResult(true);
					}
				}
			}, null, TimeSpan.Zero, interval);

			await foreach (var item in input) {
				lock (locker) {
					buffer.Add(item);
					if (buffer.Count >= batchSize && !batchReady.Task.IsCompleted) {
						batchReady.TrySetResult(true);
					}
				}

				if (batchReady.Task.IsCompleted) {
					T[] currentBatch;
					lock (locker) {
						currentBatch = buffer.ToArray();
						buffer.Clear();
					}
					yield return currentBatch;

					lastYieldTime = DateTime.UtcNow;
					batchReady = new TaskCompletionSource<bool>();
				}
			}

			await timer.DisposeAsync();

			if (buffer.Any()) {
				lock (locker) {
					yield return buffer.ToArray();
					buffer.Clear();
				}
			}
		}
	}





	//public class AsyncProcessorB<T> {

	//	private readonly object _locker = new();
	//	private Timer? _timer;

	//	public AsyncProcessorB(IAsyncEnumerable<T> items, long batchSize, TimeSpan interval) {

	//	}
	//}

	public class AsyncProcessor<T> {

		private readonly object _locker = new();
		private Timer? _timer;

		public virtual async Task ProcessAsync(IAsyncEnumerable<T> items, Func<IEnumerable<T>, Task> operation, long batchSize, TimeSpan interval) {

			var buffer = new List<T>();

			_timer = new Timer(async s => await TimerCallback((IList<T>)s!, operation), buffer, Timeout.Infinite, Timeout.Infinite);

			await foreach (var item in items) {
				lock (_locker) {
					buffer.Add(item);
				}

				if (buffer.Count >= batchSize) {
					await operation(GetBatch(buffer));
				}

				_timer.Change((int)interval.TotalMilliseconds, Timeout.Infinite);
			}

			_timer.Dispose();
			if (buffer.Any()) {
				await operation(GetBatch(buffer));
			}
		}

		protected IEnumerable<T> GetBatch(IList<T> batch) {
			T[] itemsToProcess;
			lock (_locker) {
				itemsToProcess = batch.ToArray();
				batch.Clear();
			}
			return itemsToProcess;
		}

		protected virtual async Task TimerCallback(IList<T>? batch, Func<IEnumerable<T>, Task> operation) {
			if (batch == null)
				return;
			var itemsToProcess = GetBatch(batch);
			if (itemsToProcess.Any())
				await operation(itemsToProcess);
		}
	}
}