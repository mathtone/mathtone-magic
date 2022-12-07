using Mathtone.Sdk.Common.Extensions;
using System.Collections.Concurrent;
using System.Text;

namespace Mathtone.Sdk.Rng {

	[Flags]
	public enum AlphaNumInclude : byte {
		Lower = 1 << 0,
		Upper = 1 << 1,
		Number = 1 << 2,
		Symbol = 1 << 3,
		All = Lower | Upper | Number | Symbol,
		Alpha = Lower | Upper,
		AlphaNum = Alpha | Number,
		AlphaNumUpper = Upper | Number,
		AlphaNumLower = Lower | Number
	}

	public static class IRandomExtensions {

		static readonly char[] lower = "abcdefghijklmnopqrstuvwxyz".ToArray();
		static readonly char[] upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToArray();
		static readonly char[] number = "1234567890".ToArray();
		static readonly char[] symbol = "!@#$%^&*_-=+".ToArray();

		static readonly ConcurrentDictionary<AlphaNumInclude, char[]> _cache = new();

		public static Guid NewGuid(this IRandom rng) => new(rng.GetBytes(16));

		public static string GetString(this IRandom rng, int length, AlphaNumInclude include = AlphaNumInclude.All) =>
			new(Repeat.For(length, () => rng.Random(_cache.GetOrAdd(include, GetChars(include)))).ToArray());

		public static string Base64String(this IRandom rng, int bytes) => Convert.ToBase64String(rng.GetBytes(bytes));

		public static string HexString(this IRandom rng, int bytes) => BitConverter.ToString(rng.GetBytes(bytes)).Replace("-", "");


		public static IEnumerable<T> Randomize<T>(this IRandom rng, IList<T> items) {

			var indices = Enumerable.Range(0, items.Count).ToArray();
			rng.UnSort(indices);
			return indices.Select(i => items[i]);
		}

		static char[] GetChars(AlphaNumInclude include) {
			IEnumerable<char> chars = Array.Empty<char>();
			if (include.HasFlag(AlphaNumInclude.Lower))
				chars = chars.Concat(lower);

			if (include.HasFlag(AlphaNumInclude.Upper))
				chars = chars.Concat(upper);

			if (include.HasFlag(AlphaNumInclude.Number))
				chars = chars.Concat(number);

			if (include.HasFlag(AlphaNumInclude.Symbol))
				chars = chars.Concat(symbol);
			return chars.ToArray();
		}

		public static bool NextBool(this IRandom rng) =>
			Convert.ToBoolean(rng.Next(2));


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