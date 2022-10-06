namespace Mathtone.Sdk.Rng.Tests {
	public class IRandomExtensionsTests {

		IRandom _rng = new SimpleRandom(100);

		[Fact]
		public void Random_FromList() {
			var items = Enumerable.Range(0, 10).ToList();
			var r = _rng.Random(items);
			Assert.Equal(9, r);
		}

		[Fact]
		public void Random_FromEnumerable() {
			var items = Enumerable.Range(0, 10);
			var r = _rng.Random(items);
			Assert.Equal(9, r);
		}

		[Fact]
		public void Randomize_List() {
			var items = Enumerable.Range(0, 5).ToList();
			var r = _rng.Randomize(items).ToArray();
			Assert.Equal(new[] { 3, 1, 2, 0, 4 }, r);
		}

		[Fact]
		public void Randomize_IEnumerable() {
			var items = Enumerable.Range(0, 5);
			var r = _rng.Randomize(items).ToArray();
			Assert.Equal(new[] { 3, 1, 2, 0, 4 }, r);
		}
	}
}