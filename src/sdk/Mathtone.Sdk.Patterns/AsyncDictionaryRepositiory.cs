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

		Task IAsyncRepository<ID, ITEM>.Update(ITEM item) => Task.Run(() => {
			var id = IdSelector(item);
			Items[id] = item;
		});
	}
}