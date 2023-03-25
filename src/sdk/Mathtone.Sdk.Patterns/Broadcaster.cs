using Mathtone.Sdk.Common;
using Mathtone.Sdk.Patterns;
using System.Threading.Channels;

namespace Mathtone.Sdk.Patterns {


	public class Broadcaster<T> : AsyncDisposableBase, IBroadcaster<T> {

		public Broadcaster() {
			_processTask = Task.Run(async () => {
				await foreach (var i in _channel.Reader.ReadAllAsync()) {
					await Task.WhenAll(
						subscribers.Select(s => 
						s.SendAsync(i).AsTask())
					);
				}
			});
		}
		bool _isSet;
		readonly Task _processTask;
		readonly List<Subscriber<T>> subscribers = new();
		readonly Channel<T> _channel = Channel.CreateUnbounded<T>();
		public T? Last { get; protected set; } 

		public virtual ValueTask Send(T item) {
			lock (subscribers) {
				_isSet = true;
				Last = item;
			}
			return _channel.Writer.WriteAsync(item);
		}

		public virtual ISubscriber<T> Subscribe() {
			var rtn = new Subscriber<T>();
			lock (subscribers) {
				//if (_isSet) {
				//	rtn.SendAsync(Last);
				//}
				rtn.Closing += Rtn_Closing;
				subscribers.Add(rtn);
			}
			return rtn;
		}

		private void Rtn_Closing(object? sender, EventArgs e) {
			var s = (Subscriber<T>)sender!;
			s.Closing -= Rtn_Closing;
			subscribers.Remove(s);
			
		}

		protected override async ValueTask OnDisposeAsync() {
			await base.OnDisposeAsync();
			_channel.Writer.TryComplete();
			await _processTask;
			await Task.WhenAll(
				subscribers.Select(s => Task.Run(() => s.Close()))
			);

		}
	}



	public interface IBroadcaster<T> : ISubscribable<T>, IAsyncDisposable {
		ValueTask Send(T item);

	}
	public interface ISubscribable<T> {
		ISubscriber<T> Subscribe();
	}
}