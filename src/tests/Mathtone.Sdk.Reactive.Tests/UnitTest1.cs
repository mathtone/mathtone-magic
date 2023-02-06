namespace Mathtone.Sdk.Reactive.Tests {
	public class UnitTest1 {
		
		[Fact]
		public void Test1() {
			var items = new List<int>();
			var val = 0.CreateObservable();
			val.Observe.Subscribe(items.Add);
			val.Value = 1;
			val.Value = 2;
			val.Value = 3;
			Assert.Equal(new[] { 0, 1, 2, 3 }, items);

		}
	}
}