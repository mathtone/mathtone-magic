namespace Mathtone.Sdk.Common.Extensions {
	public static class IEnumerableExtensions {
		public static void ForEach<T>(this IEnumerable<T> items, Action<T> action) {
			foreach (var i in items) {
				action(i);
			}
		}

		public static async Task ForEachAsync<T>(this IEnumerable<T> items, Func<T,Task> action) {
			foreach (var i in items) {
				await action(i);
			}
		}

	}

	public static class IListExtensions {
		public static void Swap<T>(this IList<T> items, int a, int b) =>
			(items[b], items[a]) = (items[a], items[b]);
	}

	public static class Repeat {

		public static IEnumerable<T> For<T>(int count, Func<T> func) => While(() => count-- > 0, func);

		public static IEnumerable<T> While<T>(Func<bool> condition, Func<T> func) {
			while (condition()) {
				yield return func();
			}
		}

		public static void While(Func<bool> condition, Action action) {
			while (condition()) {
				action();
			}
		}

		public static async Task AwaitWhile(Func<bool> condition, Func<Task> action) {
			while (condition()) {
				await action();
			}
		}

		public static async Task AwaitWhile(Func<Task<bool>> condition, Func<Task> action) {
			while (await condition()) {
				await action();
			}
		}
	}
}