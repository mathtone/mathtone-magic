using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Mathtone.Sdk.Reactive {

	public class ValueChangedEventArgs<T> : EventArgs {
		public ValueChangedEventArgs(T oldValue, T newValue) =>
			(OldValue, NewValue) = (oldValue, newValue);

		public T OldValue { get; }
		public T NewValue { get; }
	}
}