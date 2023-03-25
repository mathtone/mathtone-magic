namespace Mathtone.Sdk.Common.Extensions {
	public static class Repeat {

		public static IEnumerable<T> For<T>(int count, Func<T> func) {
			for(var i = 0; i < count; i++) {
				yield return func();
			}
		}

		public static IEnumerable<T> While<T>(Func<bool> condition, Func<T> func) {
			while (condition()) {
				yield return func();
			}
		}

		public static void While(Func<bool> condition, Action action) {
			while (condition()) {
				action();
			}
		}

		public static async Task AwaitWhile(Func<bool> condition, Func<Task> action) {
			while (condition()) {
				await action();
			}
		}

		public static async Task AwaitWhile(Func<Task<bool>> condition, Func<Task> action) {
			while (await condition()) {
				await action();
			}
		}
	}
}