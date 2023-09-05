using System.Runtime.CompilerServices;

namespace _Sandbox.AsyncProcessor {

	public class TestClass {

		//[Fact]
		public async Task TestMethod() {
			//var
			var items = GetValues(1000, i => i == 333 ? TimeSpan.FromMilliseconds(1000) : TimeSpan.Zero).GetAsyncEnumerator();
			var batch1 = await items.TakeAsync(500, TimeSpan.FromMilliseconds(100), default).ToArrayAsync();
			var batch2 = await items.TakeAsync(500, TimeSpan.FromMilliseconds(100), default).ToArrayAsync();
			var batch3 = await items.TakeAsync(500, TimeSpan.FromMilliseconds(100), default).ToArrayAsync();

			var l = new[] { batch1, batch2, batch3 }.Sum(b => b.Length);
			Assert.Equal(1000, l);
			;
		}

		static async IAsyncEnumerable<int> GetValues(int valueCount, Func<int, TimeSpan> delayFunc) {
			foreach (var i in Enumerable.Range(1, valueCount)) {
				var span = delayFunc(i);
				if (span != TimeSpan.Zero) {
					await Task.Delay(span);
				}
				yield return i;
			}
		}
	}
	public static class AsyncEnumerableExtensions {

		public static async IAsyncEnumerable<T> TakeAsync<T>(this IAsyncEnumerator<T> source, int count, TimeSpan timeout, [EnumeratorCancellation] CancellationToken cancellationToken = default) {
			for (var i = 0; i < count; i++) {
				T current = default!;
				try {
					var rslt = await source
						.MoveNextAsync()
						.AsTask()
						.WaitAsync(timeout, cancellationToken);
					if (!rslt) {
						break;
					}
					else {
						current = source.Current;
					}
				}
				catch (TimeoutException) {
					if (!current!.Equals(source.Current)) {
						current = source.Current;
					}
					
					break;
				}
				yield return current;
			}


			//if (rslt) {
			//		yield return source.Current;
			//	}
			//	else {
			//		yield break;
			//	}
			//}


		}

		//await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)) {
		//	yield return item;
		//	if (--count == 0) {
		//		break;
		//	}
		//}
	}
}
