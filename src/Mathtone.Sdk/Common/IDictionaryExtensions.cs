namespace Mathtone.Sdk.Common {
	public static class IDictionaryExtensions {
		public static IRepository<ID, T> ToRepository<ID, T>(this IDictionary<ID, T> items, Func<T, ID> idSelector) =>
			new DictionaryRepo<ID, T>(items, idSelector);

		public static IRepository<ID, T> ToRepository<ID, T>(this IDictionary<ID, T> items)
			where T : IIdentified<ID> =>
			new DictionaryRepo<ID, T>(items, i => i.Id);
	}
}