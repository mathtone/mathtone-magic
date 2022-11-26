namespace Mathtone.Sdk.Patterns {
	public interface IAsyncRepository<ID, ITEM> {
		ValueTask<ID> Create(ITEM item);
		ValueTask<ITEM> Read(ID id);
		Task Update(ITEM item);
		Task Delete(ID id);
	}
	public interface IAsyncListRepository<ID, ITEM> : IAsyncRepository<ID, ITEM> {
		IAsyncEnumerable<ITEM> ReadAll();
	}
}

