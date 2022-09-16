namespace Mathtone.Sdk.Common {
	public class DictionaryRepo<ID, T> : IRepository<ID, T> {

		readonly IDictionary<ID, T> _items;
		readonly Func<T, ID> _idSelector;

		public DictionaryRepo(IDictionary<ID, T> items, Func<T, ID> idSelector) =>
			(_items, _idSelector) = (items, idSelector);

		public T Read(ID id) => _items[id];

		public void Update(T entity) => _items[_idSelector(entity)] = entity;

		public bool Delete(ID id) => _items.Remove(id);

		public ID Create(T entity) {
			var id = _idSelector(entity);
			_items.Add(id, entity);
			return id;
		}
	}
}