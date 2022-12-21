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
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
