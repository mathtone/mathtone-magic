namespace Mathtone.Sdk.Common.Tests {
	public class UnitTest1 {
		[Fact]
		public void Test1() {
			var rslt = new List<int>();
			var items = new[] { 1, 2, 3 };
			items.ForEach(rslt.Add);
			Assert.Equal(items, rslt);
		}
	}
}