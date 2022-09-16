namespace Mathtone.Sdk.Common {
	public interface IRepository<ID, T> {
		ID Create(T entity);
		T Read(ID id);
		void Update(T entity);
		bool Delete(ID id);
	}
}