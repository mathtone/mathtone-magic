﻿namespace Mathtone.Sdk.Common.Extensions {
	public static class IEnumerableExtensions {
		public static void ForEach<T>(this IEnumerable<T> items, Action<T> action) {
			foreach (var i in items) {
				action(i);
			}
		}
	}
}