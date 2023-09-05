using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace _Sandbox.AsyncProcessor {
	public class BatchStreamTests {
		[Fact]
		public async Task BatchStream_ReturnsBatches() {
			// Arrange
			var input = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }.ToAsyncEnumerable();
			var batchSize = 3;
			var interval = TimeSpan.FromMilliseconds(100);
			var expected = new[] { new[] { 1, 2, 3 }, new[] { 4, 5, 6 }, new[] { 7, 8, 9 }, new[] { 10 } };

			// Act
			var result = await input.GetBatches(batchSize, batchSize * 2, interval, default).ToListAsync();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public async Task BatchStream_ReturnsBatchesInTime() {
			// Arrange

			var input = WithDelay(10, TimeSpan.FromMilliseconds(1000), 3);
			var batchSize = 55;
			var interval = TimeSpan.FromMilliseconds(100);
			var expected = new[] { new[] { 1, 2 }, new[] { 3, 4, 5 }, new[] { 6, 7, 8 }, new[] { 9, 10 } };

			// Act
			var result = await input.GetBatches(batchSize, batchSize * 2, interval, default).ToListAsync();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public async Task BatchStream_ReturnsBatchesInTime_1ms() {
			// Arrange

			var input = WithDelay(1000, TimeSpan.FromMilliseconds(10), 10);
			var batchSize = 500;
			var interval = TimeSpan.FromMilliseconds(30);
			// Act
			var result = await input.GetBatches(batchSize, batchSize * 2, interval, default).ToListAsync();
			//Assert.Equal(4, result.Count);
			Assert.Equal(1000, result.Sum(r => r.Length));
			;
		}

		[Fact]
		public async Task BatchStream_ReturnsBatchesInTime_10ms() {
			// Arrange

			var input = WithDelay(1000, TimeSpan.FromMilliseconds(100), 600);
			var batchSize = 500;
			var interval = TimeSpan.FromMilliseconds(10);
			// Act
			var result = await input.GetBatches(batchSize, batchSize * 2, interval, default).ToListAsync();
			//Assert.Equal(4, result.Count);
			Assert.Equal(1000, result.Sum(r => r.Length));
			;
		}

		static async IAsyncEnumerable<int> WithDelay(int numbers, TimeSpan delay, int pauses) {
			foreach (var i in Enumerable.Range(1, numbers)) {
				if (i % pauses == 0) {
					await Task.Delay(delay);
				}
				yield return i;
			}
		}
	}

	//public class AsyncBatchConverter<T> : IAsyncEnumerable<IAsyncEnumerable<T>> {

	//	private readonly long _batchSize;
	//	private readonly IAsyncEnumerable<T> _input;
	//	private readonly long? _bufferLimit;
	//	private readonly TimeSpan _interval;

	//	public AsyncBatchConverter(IAsyncEnumerable<T> input, long batchSize, TimeSpan interval, int? bufferLimit = default) {
	//		_input = input;
	//		_batchSize = batchSize;
	//		_bufferLimit = bufferLimit ?? batchSize;
	//		_interval = interval;
	//	}

	//	public async IAsyncEnumerable<IAsyncEnumerable<T>> GetAsyncEnumerator([EnumeratorCancellation] CancellationToken cancellationToken = default) {
	//		var  channel = Channel.CreateUnbounded<T>();
	//		var intake = Task.Run(async () => {
	//			await foreach (var i in _input) {
	//				await channel.Writer.WriteAsync(i, cancellationToken);
	//			}
	//			channel.Writer.Complete();
	//		}, cancellationToken);

	//		await foreach (var i in channel.Reader.ReadAllAsync()) {
	//			throw new NotImplementedException();
	//		}
			
	//	}

	//	IAsyncEnumerator<IAsyncEnumerable<T>> IAsyncEnumerable<IAsyncEnumerable<T>>.GetAsyncEnumerator(CancellationToken cancellationToken) {
	//		throw new NotImplementedException();
	//	}
	//}

	public static class StreamingExtensions {

		public static async IAsyncEnumerable<T[]> GetBatches<T>(this IAsyncEnumerable<T> input, int batchSize, int bufferLimit, TimeSpan interval, [EnumeratorCancellation] CancellationToken cancellationToken = default) {


			var complete = false;
			var channel = Channel.CreateUnbounded<T>();
			using var batchWait = new AutoResetEvent(false);
			using var intakeWait = new AutoResetEvent(false);
			await using var timer = new Timer(s => batchWait.Set(), null, interval, Timeout.InfiniteTimeSpan);

			var intake = Task.Run(async () => {
				await foreach (var i in input) {
					timer.Change(interval, Timeout.InfiniteTimeSpan);
					await channel.Writer.WriteAsync(i, cancellationToken);
					if (channel.Reader.Count >= batchSize) {
						batchWait.Set();
						if (channel.Reader.Count >= bufferLimit) {
							intakeWait.WaitOne();
						}
					}
				}
				complete = true;
				channel.Writer.Complete();
			}, cancellationToken);

			while (!complete || channel.Reader.Count > 0) {

				if (!complete && channel.Reader.Count < batchSize) {
					batchWait.WaitOne();
				}

				var thisBatchSize = channel.Reader.Count >= batchSize ? batchSize : channel.Reader.Count;
				var thisBatch = new List<T>();
				for (var i = 0; i < thisBatchSize; i++) {
					channel.Reader.TryRead(out var item);
					if (item != null) {
						thisBatch.Add(item);
					}
					else {
						break;
					}
				}

				if (thisBatch.Any())
					yield return thisBatch.ToArray();

				intakeWait.Set();
			}

			await intake;
		}
	}
}