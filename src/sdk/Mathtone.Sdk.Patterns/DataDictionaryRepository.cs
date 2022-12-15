using Mathtone.Sdk.Common;

namespace Mathtone.Sdk.Patterns {
	public class DataDictionaryRepository<ID, ITEM> : DictionaryRepository<ID, ITEM> where ID : notnull where ITEM : IIdentified<ID> {
		public DataDictionaryRepository() : base(i=>i.Id) {}
		public DataDictionaryRepository(IEnumerable<ITEM> items) : base(i => i.Id, items) {}
		public DataDictionaryRepository(IDictionary<ID, ITEM> items) : base(i => i.Id, items) {}
	}
}