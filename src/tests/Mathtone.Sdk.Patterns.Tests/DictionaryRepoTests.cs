using Mathtone.Sdk.Common;

namespace Mathtone.Sdk.Patterns.Tests {
	public class DictionaryRepoTests {

		[Fact]
		public void CreateRepository() {
		
			var repo = new[] {
				new{Id=1, Name="One"},
				new{Id=2, Name="Two"},
				new{Id=3, Name="Three"}
			}.ToRepo(i => i.Id);

			Assert.Equal("Two", repo.Read(2).Name);
			Assert.Equal(4, repo.Create(new { Id = 4, Name = "Four" }));
			Assert.Equal("Four", repo.Read(4).Name);
			
			repo.Update(new { Id = 4, Name = "Fouuuur" });
			Assert.Equal("Fouuuur", repo.Read(4).Name);
			
			repo.Delete(2);
			Assert.Throws<KeyNotFoundException>(() => repo.Read(2));

			Assert.Equal(3, repo.ReadAll().Count());

		}
	}

	public class DataDictionaryRepository<ID, ITEM> : DictionaryRepository<ID, ITEM> where ID : notnull where ITEM : IIdentified<ID> {
		public DataDictionaryRepository() : base(i => i.Id) { }
		public DataDictionaryRepository(IEnumerable<ITEM> items) : base(i => i.Id, items) { }
		public DataDictionaryRepository(IDictionary<ID, ITEM> items) : base(i => i.Id, items) { }
	}


}