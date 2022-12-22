using Mathtone.Sdk.Common;
using Mathtone.Sdk.Patterns;
using System.Threading.Channels;

namespace Mathtone.Sdk.Patterns {

	
	public class Broadcaster<T> : AsyncDisposableBase, IBroadcaster<T> {

		public Broadcaster() {
			_processTask = Task.Run(async () => {
				await foreach (var i in _channel.Reader.ReadAllAsync()) {
					await Task.WhenAll(
						subscribers.Select(s => s.Writer.WriteAsync(i).AsTask())
					);
				}
			});
		}

		readonly Task _processTask;
		readonly List<Subscriber<T>> subscribers = new();
		readonly Channel<T> _channel = Channel.CreateUnbounded<T>();


		public ValueTask Send(T item) => _channel.Writer.WriteAsync(item);

		public ISubscriber<T> Subscribe() {
			var rtn = new Subscriber<T>();
			rtn.Closing += Rtn_Closing;
			subscribers.Add(rtn);
			return rtn;
		}

		private void Rtn_Closing(object? sender, EventArgs e) {
			var s = (Subscriber<T>)sender!;
			s.Closing -= Rtn_Closing;
			subscribers.Remove(s);
			s.Writer.TryComplete();
		}

		protected override async ValueTask OnDisposeAsync() {
			await base.OnDisposeAsync();
			_channel.Writer.TryComplete();
			await _processTask;
			await Task.WhenAll(
				subscribers.Select(s => Task.Run(() => s.Writer.Complete()))
			);
		}
	}
	public interface IBroadcaster<T> : ISubscribable<T>, IAsyncDisposable {
		ValueTask Send(T item);
		
	}
	public interface ISubscribable<out T> {
		ISubscriber<T> Subscribe();
	}
}