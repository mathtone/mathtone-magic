using Mathtone.Sdk.Common;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Mathtone.Sdk.Patterns {
	public class Subscriber<T> : DisposableBase, ISubscriber<T> {

		readonly Channel<T> _channel = Channel.CreateUnbounded<T>();

		protected ChannelWriter<T> Writer => _channel.Writer;
		public ValueTask SendAsync(T item, CancellationToken cancellationToken = default) {

			return Writer.WriteAsync(item, cancellationToken);
		}
		public event EventHandler? Closing;

		public void Close() {
			var eh = Closing;
			eh?.Invoke(this, new());
			Writer.TryComplete();
		}

		public ValueTask<T> ReadAsync(CancellationToken cancellationToken = default) => _channel.Reader.ReadAsync(cancellationToken);

		public async IAsyncEnumerable<T> ReadAllAsync() {
			T? last;
			await foreach (var v in _channel.Reader.ReadAllAsync()) {
				last = v;

				yield return last;

			}
		}
		protected override void OnDisposing() {
			base.OnDisposing();
			Close();
		}
	}

	public interface ISubscriber<T> : IDisposable {
		IAsyncEnumerable<T> ReadAllAsync();
		ValueTask<T> ReadAsync(CancellationToken cancellation = default);
		void Close();
		event EventHandler Closing;
	}
}