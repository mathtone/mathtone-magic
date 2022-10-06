using System;

namespace Mathtone.Sdk.Rng.Tests {
	public class SimpleRandomTests {
		
		readonly IRandom _rng = new SimpleRandom();

		[Fact]
		public void GetBytes() =>
			Assert.Equal(10, _rng.GetBytes(10).Length);
	}
}