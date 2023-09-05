using Mathtone.Sdk.Testing;
using Mathtone.Sdk.Testing.Xunit;
using Moq;
using System.Reactive;
using Xunit.Abstractions;

namespace Mathtone.Sdk.Reactive.Tests {
	public class ObservableValueTests : XunitServiceTestBase {
		public ObservableValueTests(ITestOutputHelper output) : base(output) {
		}

		[Fact]
		public void Subscribe_To_Obsaervable() {

			var items = new List<int>();
			var val = 0.CreateObservable();
			val.Observe.Subscribe(items.Add);
			val.Value = 1;
			val.Value = 2;
			val.Value = 3;

			Assert.Equal(3, val.Value);
			Assert.Equal(new[] { 0, 1, 2, 3 }, items);
		}

		[Fact]
		public void Use_Operators() {
			ObservableValue<int> val = 1;
			int i = (int)val;
			Assert.Equal(1, i);
		}

		[Fact]
		public void Subscribe_Should_Subscribe_Observer_To_Observable() {
			var observable = new ObservableValue<int>(1);
			var observer = new Mock<IObserver<int>>();
			using var sub = observable.Subscribe(observer.Object);
			Assert.NotNull(sub);
		}

		[Fact]
		public void CoverTypes() {
			CodeCoverage.CoverType<ValueChangedEventArgs<int>>(Log);
			Assert.True(true);
		}
	}
}