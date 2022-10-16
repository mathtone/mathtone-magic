namespace Mathtone.Sdk.Patterns {

	public class DictionaryRepository<ID, ITEM> : IListRepository<ID, ITEM> where ID : notnull {

		public DictionaryRepository(Func<ITEM, ID> idSelector) : this(idSelector, Array.Empty<ITEM>()) { }
		public DictionaryRepository(Func<ITEM, ID> idSelector, IEnumerable<ITEM> items) : this(idSelector, items.ToDictionary(idSelector)) { }
		public DictionaryRepository(Func<ITEM, ID> idSelector, IDictionary<ID, ITEM> items) {
			_idSelector = idSelector;
			_items = items;
		}
		readonly Func<ITEM, ID> _idSelector;
		readonly IDictionary<ID, ITEM> _items;

		public ID Create(ITEM item) {
			var id = _idSelector(item);
			_items.Add(id, item);
			return id;
		}

		public ITEM Read(ID id) => _items[id];

		public void Update(ITEM item) => _items[_idSelector(item)] = item;

		public void Delete(ID id) => _items.Remove(id);

		public IEnumerable<ITEM> ReadAll() => _items.Values;
	}
}