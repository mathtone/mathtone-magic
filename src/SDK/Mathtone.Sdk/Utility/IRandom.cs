using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Mathtone.Sdk.Utility {

	public interface IRandom {
		int Next();
		int Next(int maxValue);
		int Next(int minValue, int maxValue);
		double NextDouble();
		byte[] GetBytes(int count);
		void NextBytes(byte[] values);
		uint NextUint();
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "I will it")]
	public static class RNG {
		static IRandom? _default;
		static IRandom? _secure;
		public static IRandom Default => _default ??= new SimpleRng();
		public static IRandom Secure => _secure ??= new CryptoRng();
	}

	public static class IRandomExtensions {
		const string ALPHA_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

		

		public static string GetString(this IRandom rng, int length) {

			var chars = new char[length];

			for (int i = 0; i < chars.Length; i++) {
				chars[i] = ALPHA_CHARS[rng.Next(ALPHA_CHARS.Length)];
			}

			return new string(chars);
		}

		public static IEnumerable<T> Randomize<T>(this IRandom randomSource, IList<T> items) {
			var indices = Enumerable.Range(0, items.Count).ToArray();
			randomSource.UnSort(indices);
			foreach (var i in indices) {
				yield return items[indices[i]];
			}
		}
		public static IEnumerable<T> Randomize<T>(this IRandom randomSource, IEnumerable<T> items) {
			var indices = Enumerable.Range(0, items.Count()).ToArray();
			randomSource.UnSort(indices);
			foreach (var i in indices) {
				yield return items.ElementAt(indices[i]);
			}
		}

		public static void UnSort<T>(this IRandom randomSource, IList<T> items) {
			for (var i = items.Count; i > 1; i--) {
				var a = randomSource.Next(i);
				var b = i - 1;
				var t = items[a];
				items[a] = items[b];
				items[b] = t;
			}
		}
		public static T Random<T>(this IRandom rng, IEnumerable<T> items) => items.Random(rng);
		public static T Random<T>(this IEnumerable<T> items, IRandom randomSource) =>
			items.ElementAt(randomSource.Next(items.Count()));

		public static T Random<T>(this IRandom rng, IList<T> items) => items.Random(rng);
		public static T Random<T>(this IList<T> items, IRandom randomSource) =>
			items[randomSource.Next(items.Count)];
	}

	public class SimpleRng : Random, IRandom {
		public SimpleRng() { }
		public SimpleRng(int seed) : base(seed) { }

		public byte[] GetBytes(int count) {
			var rtn = new byte[count];
			NextBytes(rtn);
			return rtn;
		}

		public virtual uint NextUint() {
			var buffer = new byte[sizeof(uint)];
			NextBytes(buffer);
			return BitConverter.ToUInt32(buffer, 0);
		}
	}

	public class CryptoRng : SimpleRng {

		private const long lMax = 1L + uint.MaxValue;

		public override int Next() =>
			(int)NextUint() & 0x7FFFFFFF;

		public override int Next(int maxValue) {
			if (maxValue < 0)
				throw new ArgumentOutOfRangeException(nameof(maxValue));

			return Next(0, maxValue);
		}

		public override int Next(int minValue, int maxValue) {
			if (minValue > maxValue)
				throw new ArgumentOutOfRangeException(nameof(minValue));

			if (minValue == maxValue)
				return minValue;

			var diff = (long)(maxValue - minValue);
			var remainder = maxValue % diff;
			var done = false;
			var rtn = 0;

			while (!done) {
				var r = NextUint();

				if (r < lMax - remainder) {
					rtn = (int)(minValue + (r % diff));
					done = true;
				}
			}
			return rtn;
		}

		public override double NextDouble() =>
			NextUint() / (double)lMax;

		public override void NextBytes(byte[] bytes) {
			//if (bytes == null)
			//	throw new ArgumentNullException(nameof(bytes));
			RandomNumberGenerator.Fill(bytes);
		}
	}
}