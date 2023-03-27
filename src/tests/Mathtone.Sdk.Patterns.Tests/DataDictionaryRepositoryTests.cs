using Mathtone.Sdk.Common;

namespace Mathtone.Sdk.Patterns.Tests {
	public class DataDictionaryRepositoryTests {
		[Fact]
		public void Constructor_NoParameters_ShouldCreateEmptyRepository() {
			// Arrange
			var repository = new DataDictionaryRepository<int, TestItem>();

			// Act
			var count = repository.ReadAll().Count();

			// Assert
			Assert.Equal(0, count);
		}

		[Fact]
		public void Constructor_EnumerableParameter_ShouldCreateRepositoryWithItems() {
			// Arrange
			var items = new List<TestItem>
			{
				new TestItem { Id = 1 },
				new TestItem { Id = 2 },
				new TestItem { Id = 3 }
			};
			var repository = new DataDictionaryRepository<int, TestItem>(items);

			// Act
			var count = repository.ReadAll().Count();

			// Assert
			Assert.Equal(3, count);
		}

		[Fact]
		public void Constructor_DictionaryParameter_ShouldCreateRepositoryWithItems() {
			// Arrange
			var items = new Dictionary<int, TestItem>
			{
				{ 1, new TestItem { Id = 1 } },
				{ 2, new TestItem { Id = 2 } },
				{ 3, new TestItem { Id = 3 } }
			};
			var repository = new DataDictionaryRepository<int, TestItem>(items);

			// Act
			var count = repository.ReadAll().Count();

			// Assert
			Assert.Equal(3, count);
		}

		public class TestItem : IIdentified<int> {
			public int Id { get; set; }
		}
	}
}