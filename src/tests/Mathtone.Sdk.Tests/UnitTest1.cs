using Mathtone.Sdk.Common;

namespace Mathtone.Sdk.Tests {
	public class TestValue : Identified<int> {
		public TestValue(int id, string value=default) : base(id) {
			Value = value;
		}

		public string Value { get; set; }
	}
	public class DictionaryRepositoryTests {

		readonly IRepository<int, TestValue> _repositiory;

		public DictionaryRepositoryTests() {
			var itemId = 3;
			_repositiory = new Dictionary<int, TestValue>(){
				{1,new(1) },
				{2,new(2) },
				{3,new(3) }
			}.ToRepository(i => i.Id = ++itemId);
		}

		[Fact]
		public void Create() =>
			Assert.Equal(4, _repositiory.Create(new(4)));

		[Fact]
		public void Read() =>
			Assert.Equal(3, _repositiory.Read(3).Id);

		[Fact]
		public void Update() {
			_repositiory.Update(new(3) { Value = "Test" });
			Assert.Equal("Test", _repositiory.Read(3).Value);
		}

		[Fact]
		public void Delete() {
			_repositiory.Delete(3);
			Assert.Throws<KeyNotFoundException>(() => _repositiory.Read(3));
		}
	}
}