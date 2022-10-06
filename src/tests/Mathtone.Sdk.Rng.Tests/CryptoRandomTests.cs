namespace Mathtone.Sdk.Rng.Tests {
	public class CryptoRandomTests {
		CryptoRandom _rng = new();

		[Fact]
		public void GetBytes() =>
			Assert.Equal(10, _rng.GetBytes(10).Length);

		[Fact]
		public void Next() {
			var r = _rng.Next();
			Assert.True(r >= int.MinValue && r <= int.MaxValue);
		}

		[Fact]
		public void NextDouble() {
			var r = _rng.NextDouble();
			Assert.True(r >= 0.0 && r <= 1.0);
		}

		[Fact]
		public void Next_MaxValue() {
			var r = _rng.Next(5);
			Assert.True(r <= 5);
		}


		[Fact]
		public void Next_NoSpread() {
			var r = _rng.Next(2, 2);
			Assert.Equal(2, r);
		}

		//[Fact]
		//public void NextBytes_Throws_1() =>
		//	Assert.Throws<ArgumentNullException>(() => _rng.NextBytes(null));

		[Fact]
		public void Next_Throws_1() =>
			Assert.Throws<ArgumentOutOfRangeException>(() => _rng.Next(-5));

		[Fact]
		public void Next_Throws_2() =>
			Assert.Throws<ArgumentOutOfRangeException>(() => _rng.Next(5, 4));

	}
}