using System.Data.SqlClient;
using Xunit.Sdk;
using Mathtone.Sdk.Data.Sql;
using System.Data;
using Mathtone.Sdk.Data.Npgsql.Tests;
using Mathtone.Sdk.Data.Tests;

namespace Mathtone.Sdk.Data.Sql.Tests {

	public class SqlConnectionExtensionsTests : IDbConnectionExtensionsTests<SqlConnection, SqlCommand> {

		[Fact]
		public void CreateCommand_Returns_Command() {
			var cmd = new SqlConnection().CreateCommand("TEST", CommandType.StoredProcedure);
			Assert.Equal("TEST", cmd.CommandText);
			Assert.Equal(CommandType.StoredProcedure, cmd.CommandType);
		}

		protected override SqlConnection CreateConnection() => CreateConnection(($"Server=localhost;User Id=sa;Password=test!1234"));

		protected override SqlConnection CreateConnection(string connectionString) => new(connectionString);

		protected override SqlCommand CreateCommand(string commandText) => new(commandText);

		protected override string GetDateQuery => "SELECT GETDATE() as Value";

		protected override string Get123Query => "SELECT value FROM STRING_SPLIT('1 2 3', ' ');";
	}

	public class SqlCommandExtensionsTests {

		[Fact]
		public void CreateCommand_Returns_Command() {
			var cmd = new SqlCommand();
			var now = DateTimeOffset.Now;
			cmd.WithInput("@Test", now, SqlDbType.DateTimeOffset);
			var p = cmd.Parameters["@Test"];
			Assert.Equal(SqlDbType.DateTimeOffset, p.SqlDbType);
			Assert.Equal(now, p.Value);
		}
	}

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

		protected SqlConnection CreateConnection() => CreateConnection(($"Server=localhost;User Id=sa;Password=test!1234"));

		protected SqlConnection CreateConnection(string connectionString) => new(connectionString);
	}
}