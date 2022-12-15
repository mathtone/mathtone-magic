using System.Runtime.CompilerServices;

namespace Mathtone.Sdk.Patterns {

	public class AsyncDictionaryRepositiory<ID, ITEM> : DictionaryRepository<ID, ITEM>, IAsyncListRepository<ID, ITEM> where ID : notnull {
		public AsyncDictionaryRepositiory(Func<ITEM, ID> idSelector) : base(idSelector) {
		}

		public AsyncDictionaryRepositiory(Func<ITEM, ID> idSelector, IEnumerable<ITEM> items) : base(idSelector, items) {
		}

		public AsyncDictionaryRepositiory(Func<ITEM, ID> idSelector, IDictionary<ID, ITEM> items) : base(idSelector, items) {
		}

		ValueTask<ID> IAsyncRepository<ID, ITEM>.Create(ITEM item) =>new(base.Create(item));

		Task IAsyncRepository<ID, ITEM>.Delete(ID id) => Task.Run(() => base.Delete(id));

		ValueTask<ITEM> IAsyncRepository<ID, ITEM>.Read(ID id) =>new(base.Read(id));

		async IAsyncEnumerable<ITEM> IAsyncListRepository<ID, ITEM>.ReadAll() {
			foreach (var i in base.ReadAll()) {
				yield return i;
			}
			await Task.CompletedTask;
		}

		Task IAsyncRepository<ID, ITEM>.Update(ITEM item) {
			throw new NotImplementedException();
		}
	}

	public class DictionaryRepository<ID, ITEM> : IListRepository<ID, ITEM> where ID : notnull {

		public DictionaryRepository(Func<ITEM, ID> idSelector) : this(idSelector, Array.Empty<ITEM>()) { }
		public DictionaryRepository(Func<ITEM, ID> idSelector, IEnumerable<ITEM> items) : this(idSelector, items.ToDictionary(idSelector)) { }
		public DictionaryRepository(Func<ITEM, ID> idSelector, IDictionary<ID, ITEM> items) {
			_idSelector = idSelector;
			Items = items;
		}
		readonly Func<ITEM, ID> _idSelector;
		protected readonly IDictionary<ID, ITEM> Items;

		public virtual ID Create(ITEM item) {
			var id = _idSelector(item);
			Items.Add(id, item);
			return id;
		}

		public virtual ITEM Read(ID id) => Items[id];

		public virtual void Update(ITEM item) => Items[_idSelector(item)] = item;

		public virtual void Delete(ID id) => Items.Remove(id);

		public virtual IEnumerable<ITEM> ReadAll() => Items.Values;
	}
}