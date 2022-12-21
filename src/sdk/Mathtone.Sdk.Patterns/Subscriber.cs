﻿using System.Threading.Channels;

namespace Mathtone.Sdk.Patterns {
	public class Subscriber<T> : ISubscriber<T> {
		readonly Channel<T> _channel = Channel.CreateUnbounded<T>();

		public ChannelWriter<T> Writer => _channel.Writer;

		public IAsyncEnumerable<T> ReadAllAsync() => _channel.Reader.ReadAllAsync();
	}

	public interface ISubscriber<T> {
		IAsyncEnumerable<T> ReadAllAsync();
	}
}