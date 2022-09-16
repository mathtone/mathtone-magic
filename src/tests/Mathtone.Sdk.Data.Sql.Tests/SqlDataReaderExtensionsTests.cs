using System.Data.SqlClient;

namespace Mathtone.Sdk.Data.Sql.Tests {
	public class SqlDataReaderExtensionsTests {
		[Fact]
		public async Task ConsumeAsync_1() {
			using var cn = CreateConnection();
			cn.Open();
			var rslt = await (await cn.CreateCommand("SELECT value FROM STRING_SPLIT('1 2 3', ' ');")
				.ExecuteReaderAsync())
				.ConsumeAsync(r => Task.Run(()=>r.Field<int>(0)))
				.ToArrayAsync();


			Assert.Equal(3, rslt.Length);
		}

		[Fact]
		public async Task ConsumeAsync_2() {
			using var cn = CreateConnection();
			cn.Open();
			var rslt = await (await cn.CreateCommand("SELECT value FROM STRING_SPLIT('1 2 3', ' ');")
				.ExecuteReaderAsync())
				.ConsumeAsync(r => r.Field<int>(0))
				.ToArrayAsync();
			Assert.Equal(3, rslt.Length);
		}

		protected SqlConnection CreateConnection() => CreateConnection($"Server=localhost;User Id=sa;Password=test!1234");

		protected static SqlConnection CreateConnection(string connectionString) => new(connectionString);
	}
}