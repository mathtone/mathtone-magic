using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Mathtone.Sdk.Reactive {
	public struct ObservableValue<T> : IObservableValue<T> {


		public ObservableValue(T value) => _value = value;

		T _value;
		readonly Subject<T> _subject = new();

		public T Value { readonly get => _value;
			set => _subject.OnNext(_value = value);
		}

		public IObservable<T> Observe => _subject.AsObservable().Prepend(_value);


		public static implicit operator T(ObservableValue<T> value) => value.Value;
		public static implicit operator ObservableValue<T>(T value) => new(value);

		public IDisposable Subscribe(IObserver<T> observer) =>
			Observe.Subscribe(observer);

	}

	public interface IObservableValue<T> : IObservable<T> {
		T Value { get; set; }
		IObservable<T> Observe { get; }
	}

	//public class ValueChangedEventArgs<T> : EventArgs {
	//	public ValueChangedEventArgs(T oldValue, T newValue) =>
	//		(OldValue, NewValue) = (oldValue, newValue);

	//	public T OldValue { get; }
	//	public T NewValue { get; }
	//}
}