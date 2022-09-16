namespace Mathtone.Sdk.Common {
	public interface IAsyncRepository<ID, T> {
		Task<ID> CreateAsync(T entity);
		Task<T> ReadAsync(ID id);
		Task UpdateAsync(T entity);
		Task<bool> DeleteAsync(ID id);
	}
}