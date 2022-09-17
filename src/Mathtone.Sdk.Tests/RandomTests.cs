using Mathtone.Sdk.Utility;

namespace Mathtone.Sdk.Tests {
	public class SimpleRngTests {
		[Fact]
		public void Create_WithSeed() {
			var r1 = new Random(1);
			var r2 = new SimpleRng(1);

			Assert.Equal(r1.Next(), r2.Next());
		}

		[Fact]
		public void GetBytes() {
			Assert.Equal(100, RNG.Default.GetBytes(100).Length);
		}
	}

	public class CryptoRngTests {
		[Fact]
		public void Next_1() {
			Assert.NotEqual(0, RNG.Secure.Next());
		}

		[Fact]
		public void Next_2() {
			Assert.True(RNG.Secure.Next(2) < 2);
			Assert.Equal(2, RNG.Secure.Next(2,2));
			Assert.Throws<ArgumentOutOfRangeException>(() => _ = RNG.Secure.Next(1,0));
			Assert.Throws<ArgumentOutOfRangeException>(() => _ = RNG.Secure.Next(-1));
		}

		[Fact]
		public void NextDouble() {
			var r = RNG.Secure.NextDouble();
			Assert.True(r >= 0 && r <= 1);
		}

		[Fact]
		public void GetBytes() {
			//Assert.Throws<ArgumentNullException>(() => _ = RNG.Secure.NextBytes(null));
			Assert.Equal(100, RNG.Secure.GetBytes(100).Length);
		}
	}

	public class RandomTests {
		[Theory, MemberData(nameof(GetRngs))]
		public void Randomize_IEnumerable(IRandom rng) {
			var values = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Where(i => true);

			Assert.NotEqual(rng.Randomize(values).ToArray(), values.OrderBy(i => i));
		}

		[Theory, MemberData(nameof(GetRngs))]
		public void Randomize_IList(IRandom rng) {
			var values = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			//rng.UnSort(values);
			Assert.NotEqual(rng.Randomize(values).ToArray(), values.OrderBy(i => i));
		}

		[Theory, MemberData(nameof(GetRngs))]
		public void Random_1(IRandom rng) {
			var values = new[] { 1, 2, 3, 4, 5 }.Where(i => true);
			Assert.Contains(rng.Random(values), values);
		}

		[Theory, MemberData(nameof(GetRngs))]
		public void Random_2(IRandom rng) {
			var values = new[] { 1, 2, 3, 4, 5 };
			Assert.Contains(rng.Random(values), values);
		}

		[Theory, MemberData(nameof(GetRngs))]
		public void GetString(IRandom rng) {
			Assert.Equal(20, rng.GetString(20).Length);
		}


		static IEnumerable<object[]> GetRngs() {
			yield return new object[] { RNG.Default };
			yield return new object[] { RNG.Secure };
		}
	}
}