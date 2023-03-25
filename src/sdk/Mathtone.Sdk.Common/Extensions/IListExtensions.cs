namespace Mathtone.Sdk.Common.Extensions {
	public static class IListExtensions {
		public static void Swap<T>(this IList<T> items, int a, int b) {
			var t = items[a];

			items[a] = items[b];
			items[b] = t;
		}
	}
}