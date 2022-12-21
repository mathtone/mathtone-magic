using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathtone.Sdk.Common {
	public class AsyncDisposableBase : IAsyncDisposable {

		public async ValueTask DisposeAsync() {
			await OnDisposeAsync().ConfigureAwait(false);
			GC.SuppressFinalize(this);
		}

		protected virtual ValueTask OnDisposeAsync() => ValueTask.CompletedTask;
	}

	public static class AsyncDisposal {
		public static Task Of(params IAsyncDisposable[] disposables) {
			return Task.WhenAll(disposables.Select(d => d.DisposeAsync().AsTask()));
		}
	}
}
