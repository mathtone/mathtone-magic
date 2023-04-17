namespace Mathtone.Sdk.Common.Tests {
	public class IdentifiedTests {
		[Fact]
		public void IsIdentified() => Assert.Equal(10, new Identified<int>(10).Id);

		[Fact]
		public void IsIdentifiedValue() {
			var v = new Identified<int,int>(1,2);
			Assert.Equal(1, v.Id);
			Assert.Equal(2, v.Value);
		}
	}
}
