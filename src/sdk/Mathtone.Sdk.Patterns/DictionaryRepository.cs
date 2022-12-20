using System.Runtime.CompilerServices;

namespace Mathtone.Sdk.Patterns {

	public class DictionaryRepository<ID, ITEM> : IListRepository<ID, ITEM> where ID : notnull {

		public DictionaryRepository(Func<ITEM, ID> idSelector) : this(idSelector, Array.Empty<ITEM>()) { }
		public DictionaryRepository(Func<ITEM, ID> idSelector, IEnumerable<ITEM> items) : this(idSelector, items.ToDictionary(idSelector)) { }
		public DictionaryRepository(Func<ITEM, ID> idSelector, IDictionary<ID, ITEM> items) {
			IdSelector = idSelector;
			Items = items;
		}
		protected Func<ITEM, ID> IdSelector { get; }
		protected readonly IDictionary<ID, ITEM> Items;

		public virtual ID Create(ITEM item) {
			var id = IdSelector(item);
			Items.Add(id, item);
			return id;
		}

		public virtual ITEM Read(ID id) => Items[id];

		public virtual void Update(ITEM item) => Items[IdSelector(item)] = item;

		public virtual void Delete(ID id) => Items.Remove(id);

		public virtual IEnumerable<ITEM> ReadAll() => Items.Values;
	}
}