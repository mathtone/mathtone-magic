namespace Mathtone.Sdk.Common {

	//public class DictionaryRepo<T> :  where T: IIdentified<Object> {

	//}

	public class DictionaryRepo<ID, T> : IRepository<ID, T> {

		readonly IDictionary<ID, T> _items;
		readonly Func<T, ID> _idSelector;
		readonly Action<T> _assignNewId;

		public DictionaryRepo(IDictionary<ID, T> items, Func<T, ID> idSelector, Action<T> assignNewId) =>
			(_items, _idSelector, _assignNewId) = (items, idSelector, assignNewId);

		public T Read(ID id) => _items[id];

		public void Update(T entity) => _items[_idSelector(entity)] = entity;

		public bool Delete(ID id) => _items.Remove(id);

		public ID Create(T entity) {
			_assignNewId(entity);
			var id = _idSelector(entity);
			_items.Add(id, entity);
			return id;
		}
	}
}