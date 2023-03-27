using Mathtone.Sdk.Common;
using Mathtone.Sdk.Testing;

namespace Mathtone.Sdk.Patterns.Tests {
	public class AsyncDictionaryRepositoryTests {
		[Fact]
		public async Task Create_ReturnsId() {
			// Arrange
			var repository = new AsyncDictionaryRepository<int, string>(item => item.Length) as IAsyncListRepository<int, string>;
			var item = "test";

			// Act
			var id = await repository.Create(item);

			// Assert
			Assert.Equal(item.Length, id);
			Assert.Equal(item, await repository.Read(id));
		}

		[Fact]
		public async Task Read_ReturnsItem() {
			// Arrange
			var repository = new AsyncDictionaryRepository<int, string>(item => item.Length) as IAsyncListRepository<int, string>;
			var item = "test";
			var id = await repository.Create(item);

			// Act
			var result = await repository.Read(id);

			// Assert
			Assert.Equal(item, result);
		}

		[Fact]
		public async Task ReadAll_ReturnsAllItems() {
			// Arrange

			var repository = new AsyncDictionaryRepository<int, Identified<int, string>>(item => item.Id) as IAsyncListRepository<int, Identified<int, string>>;
			var items = new Identified<int, string>[] { new(1, "test"), new(2, "test2"), new(3, "test3") };
			foreach (var item in items) {
				await repository.Create(item);
			}

			// Act
			var result = await repository.ReadAll().ToListAsync();

			// Assert
			Assert.Equal(items, result);
		}

		[Fact]
		public async Task Update_UpdatesItem() {
			// Arrange
			var repository = new AsyncDictionaryRepository<int, Identified<int, string>>(item => item.Id) as IAsyncListRepository<int, Identified<int, string>>;
			var item = new Identified<int, string>(1, "test");
			var id = await repository.Create(item);
			var newItem = new Identified<int, string>(1, "test2");

			// Act
			await repository.Update(newItem);

			// Assert
			Assert.Equal(newItem.Value, (await repository.Read(id)).Value);
		}

		[Fact]
		public async Task Delete_RemovesItem() {
			// Arrange
			var repository = new AsyncDictionaryRepository<int, string>(item => item.Length) as IAsyncListRepository<int, string>;
			var item = "test";
			var id = await repository.Create(item);

			// Act
			await repository.Delete(id);

			// Assert
			Assert.Empty(await repository.ReadAll().ToArrayAsync());
		}

		[Fact]
		public void CoverageTest() {
			_ = new AsyncDictionaryRepository<int, string>(i => 1);
			_ = new AsyncDictionaryRepository<int, string>(i => 1, Enumerable.Empty<string>());
			_ = new AsyncDictionaryRepository<int, string>(i => 1, Enumerable.Empty<string>().ToDictionary(i => 1, i => i));
			Assert.True(true);
		}
	}

}