namespace Mathtone.Sdk.Patterns {
	public static class IEnumerableExtensions {

		public static IListRepository<ID, ITEM> ToRepo<ID, ITEM>(this IEnumerable<ITEM> items, Func<ITEM, ID> idSelector) 
			where ID : notnull {

			var rtn = new DictionaryRepository<ID, ITEM>(idSelector);
			foreach (var i in items) {
				rtn.Create(i);
			}
			return rtn;
		}
	}
}