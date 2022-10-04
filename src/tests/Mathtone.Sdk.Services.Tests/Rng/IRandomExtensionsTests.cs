using Mathtone.Sdk.Services.Rng;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathtone.Sdk.Services.Tests.Rng {


	public class SimpleRandomTests {

		IRandom _rng = new SimpleRandom();

		[Fact]
		public void GetBytes() =>
			Assert.Equal(10, _rng.GetBytes(10).Length);
	}

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
