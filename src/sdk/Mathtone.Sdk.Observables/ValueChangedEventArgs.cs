using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Calxium.Observable {

	public class ValueChangedEventArgs<T> : EventArgs {
		public ValueChangedEventArgs(T oldValue, T newValue) =>
			(OldValue, NewValue) = (oldValue, newValue);

		public T OldValue { get; }
		public T NewValue { get; }
	}
}