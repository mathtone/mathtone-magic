﻿using System.Security.Cryptography;

namespace Mathtone.Sdk.Rng {
	public class CryptoRandom : SimpleRandom {

		private const long lMax = 1L + uint.MaxValue;
		protected readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();

		public override int Next() =>
			(int)NextUint() & 0x7FFFFFFF;

		public override int Next(int maxValue) {
			if (maxValue < 0)
				throw new ArgumentOutOfRangeException(nameof(maxValue));

			return Next(0, maxValue);
		}

		public override int Next(int min, int max) {

			if (min > max)

				throw new ArgumentOutOfRangeException(nameof(min));

			if (min == max)
				return min;

			var diff = (long)(max - min);
			var remainder = max % diff;
			var done = false;
			var rtn = 0;

			while (!done) {
				var r = NextUint();

				if (r < lMax - remainder) {
					rtn = (int)(min + r % diff);
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
			rng.GetBytes(bytes);
		}

		public virtual uint NextUint() {
			var buffer = new byte[sizeof(uint)];
			NextBytes(buffer);
			return BitConverter.ToUInt32(buffer, 0);
		}
	}
}