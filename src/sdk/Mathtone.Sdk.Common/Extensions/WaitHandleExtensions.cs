namespace Mathtone.Sdk.Common.Extensions {
	public static class WaitHandleExtensions {

		public static Task WaitOneAsync(this WaitHandle waitHandle) {
			if (waitHandle == null)
				throw new ArgumentNullException(nameof(waitHandle));

			var tcs = new TaskCompletionSource<bool>();
			var rwh = ThreadPool.RegisterWaitForSingleObject(waitHandle, (_, _) => tcs.TrySetResult(true), null, -1, true);
			var t = tcs.Task;
			tcs.Task.ContinueWith(_ => rwh.Unregister(null));
			return t;
		}
	}
}