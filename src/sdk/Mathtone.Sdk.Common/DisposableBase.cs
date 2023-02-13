namespace Mathtone.Sdk.Common {
	public abstract class DisposableBase : IDisposable {
		private bool disposedValue;

		protected virtual void Dispose(bool disposing) {
			if (!disposedValue) {
				if (disposing) {
					OnDisposing();
				}
				disposedValue = true;
			}
		}
		protected virtual void OnDisposing() { }

		public void Dispose() {
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}

	public class TestClass { }
}
