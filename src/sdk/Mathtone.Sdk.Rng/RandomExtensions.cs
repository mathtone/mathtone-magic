namespace Mathtone.Sdk.Rng {
	public static class RandomExtensions {
		public static IEnumerable<T> Randomize<T>(this IRandom rng, IList<T> items) {

			var indices = Enumerable.Range(0, items.Count).ToArray();
			rng.UnSort(indices);
			return indices.Select(i => items[i]);
		}

		public static IEnumerable<T> Randomize<T>(this IRandom rng, IEnumerable<T> items) {

			var indices = Enumerable.Range(0, items.Count()).ToArray();
			rng.UnSort(indices);
			return indices.Select(i => items.ElementAt(i));
		}

		public static void UnSort<T>(this IRandom rng, IList<T> items) {

			for (var i = items.Count; i > 1; i--) {
				var a = rng.Next(i);
				var b = i - 1;
				var t = items[a];
				items[a] = items[b];
				items[b] = t;
			}
		}

		public static T Random<T>(this IRandom rng, IList<T> items) =>
			items[rng.Next(items.Count)];

		public static T Random<T>(this IRandom rng, IEnumerable<T> items) =>
			items.ElementAt(rng.Next(items.Count()));
	}
}
