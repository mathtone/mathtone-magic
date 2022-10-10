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

		static ConcurrentDictionary<AlphaNumInclude, char[]> _cache = new();

		public static Guid NewGuid(this IRandom rng) => new(rng.GetBytes(16));

		public static string GetString(this IRandom rng, int length, AlphaNumInclude include = AlphaNumInclude.All) {
			var chars = _cache.GetOrAdd(include, GetChars(include));
			return new(Repeat.For(length, () => rng.Random(chars)).ToArray());
		}

		public static string Base64String(this IRandom rng, int bytes) => Convert.ToBase64String(rng.GetBytes(bytes));
		public static string HexString(this IRandom rng, int bytes) => BitConverter.ToString(rng.GetBytes(bytes)).Replace("-","");

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

		public static bool NextBool(this IRandom rng) {

			return Convert.ToBoolean(rng.Next(2));
			
		}
	}
}