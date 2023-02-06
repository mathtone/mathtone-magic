namespace Calxium.Observable {
	public static class ObjectExtensions {
		public static IObservableValue<T> CreateObservable<T>(this T value) {
			return new ObservableValue<T>(value);
		}
	}
}