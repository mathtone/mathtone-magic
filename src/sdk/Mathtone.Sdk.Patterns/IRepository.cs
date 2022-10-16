namespace Mathtone.Sdk.Patterns {
	public interface IRepository<ID, ITEM> {
		ID Create(ITEM item);
		ITEM Read(ID id);
		void Update(ITEM item);
		void Delete(ID id);
	}

	public interface IListRepository<ID, ITEM> : IRepository<ID, ITEM> {
		IEnumerable<ITEM> ReadAll();
	}
}

