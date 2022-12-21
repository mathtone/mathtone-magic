using Mathtone.Sdk.Common;
using System.Threading.Channels;

namespace Mathtone.Sdk.Patterns {
	public class Subscriber<T> : DisposableBase, ISubscriber<T> {


		readonly Channel<T> _channel = Channel.CreateUnbounded<T>();

		public ChannelWriter<T> Writer => _channel.Writer;

		public event EventHandler? Closing;

		public void Close() {
			var eh = Closing;
			eh?.Invoke(this, new());
		}

		public IAsyncEnumerable<T> ReadAllAsync() => _channel.Reader.ReadAllAsync();
	}

	public interface ISubscriber<out T> : IDisposable {
		IAsyncEnumerable<T> ReadAllAsync();
		void Close();
		event EventHandler Closing;
	}
}