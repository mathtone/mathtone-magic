namespace Mathtone.Sdk.Common {
	public static class IDictionaryExtensions {
		public static IRepository<ID, T> ToRepository<ID, T>(this IDictionary<ID, T> items, Func<T, ID> idSelector, Action<T> assignNewId) =>
			new DictionaryRepo<ID, T>(items, idSelector, assignNewId);

		public static IRepository<ID, T> ToRepository<ID, T>(this IDictionary<ID, T> items, Action<T> assignNewId)
			where T : IIdentified<ID> =>
			new DictionaryRepo<ID, T>(items, i => i.Id, assignNewId);
	}
}