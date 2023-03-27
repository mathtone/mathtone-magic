using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Mathtone.Sdk.Common.Tests {
	public class SequenceTests {
		[Fact]
		public void Sequence_ReturnsDates() {

			var date = new DateTime(2020, 1, 1);
			var sequence = Sequence.Create(date,d=>d.AddDays(1));
			
			// Assert
			Assert.Equal(new DateTime(2020,1,5),sequence.ElementAt(4));
		}

		[Fact]
		public void Sequence_ReturnsRange() {

			var start = new DateTime(2020, 1, 1);
			var end = new DateTime(2020, 1, 5);
			

			// Assert
			Assert.Equal(5,Sequence.Create(start,end,d=>d.AddDays(1)).Count());
		}


		[Fact]
		public void Sequence_ValidatesParameters () {

			var start = new DateTime(2020, 1, 5);
			var end = new DateTime(2020, 1, 1);


			// Assert
			Assert.Throws<ArgumentException>(()=>Sequence.Create(start,end,d=>d.AddDays(1)));
		}
	}
	public class DisposableBaseTests {
		[Fact]
		public void Dispose_CallsOnDisposing() {
			// Arrange
			var disposable = new TestDisposable();

			// Act
			disposable.Dispose();

			// Assert
			Assert.True(disposable.OnDisposingCalled);
		}

		[Fact]
		public void Dispose_SetsDisposedValueToTrue() {
			// Arrange
			var disposable = new TestDisposable();

			// Act
			disposable.Dispose();

			// Assert
			Assert.True(disposable.DisposedValue);
		}

		[Fact]
		[SuppressMessage("Major Code Smell", "S3966:Objects should not be disposed more than once", Justification = "Settle down, it's a test")]
		public void Dispose_CanBeCalledMultipleTimes() {
			// Arrange
			var disposable = new TestDisposable();

			// Act
			disposable.Dispose();
			disposable.Dispose();

			// Assert
			Assert.True(disposable.DisposedValue);
		}

		private class TestDisposable : DisposableBase {
			public bool OnDisposingCalled { get; private set; }
			public bool DisposedValue { get; private set; }
			public bool OnDisposingCalledBeforeDisposing { get; private set; }
			public bool SuppressFinalizeCalled { get; private set; }

			protected override void OnDisposing() {
				OnDisposingCalled = true;
				base.OnDisposing();
				Assert.False(DisposedValue);
			}

			protected override void Dispose(bool disposing) {
				OnDisposingCalledBeforeDisposing = OnDisposingCalled;
				base.Dispose(disposing);
				DisposedValue = true;
			}

			~TestDisposable() {
				Dispose(disposing: false);
			}
		}
	}
}
