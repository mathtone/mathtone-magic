namespace Mathtone.Sdk.Common.Extensions {
	public static class IEnumerableExtensions {
		public static void ForEach<T>(this IEnumerable<T> items, Action<T> action) {
			foreach (var i in items) {
				action(i);
			}
		}
	}

	public static class Repeat {
		public static IEnumerable<T> For<T>(int count, Func<T> func) => While(() => count-- > 0, func);
		public static IEnumerable<T> While<T>(Func<bool> condition, Func<T> func) {
			while (condition()) {
				yield return func();
			}
		}
	}
}