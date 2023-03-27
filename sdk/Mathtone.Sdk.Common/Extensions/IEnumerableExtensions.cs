namespace Mathtone.Sdk.Common.Extensions {
	public static class IEnumerableExtensions {
		public static void ForEach<T>(this IEnumerable<T> items, Action<T> action) {
			foreach (var i in items) {
				action(i);
			}
		}

		public static async Task ForEachAsync<T>(this IEnumerable<T> items, Func<T, Task> action) {
			foreach (var i in items) {
				await action(i);
			}
		}

	}
}