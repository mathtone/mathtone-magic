using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Mathtone.Sdk.Common.Tests {

	public class IdentifiedTests {
		[Fact]
		public void IsIdentified() =>Assert.Equal(10, new Identified<int>(10).Id);	
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
