using Mathtone.Sdk.Data.Tests;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using System.Data.SqlClient;

namespace Mathtone.Sdk.Data.Npgsql.Tests {

	public class NpgsqlConnectionExtensionsTests : IDbConnectionExtensionsTests<NpgsqlConnection, NpgsqlCommand> {

		[Fact]
		public void CreateCommand_Returns_Command() {
			var cmd = new NpgsqlConnection().CreateCommand("TEST", CommandType.StoredProcedure);
			Assert.Equal("TEST", cmd.CommandText);
			Assert.Equal(CommandType.StoredProcedure, cmd.CommandType);
		}

		protected override NpgsqlConnection CreateConnection(string connectionString) => new(connectionString);

		protected override NpgsqlCommand CreateCommand(string commandText) => new(commandText);

		protected override string GetDateQuery => "SELECT CURRENT_DATE as Value";

		protected override NpgsqlConnection CreateConnection() => CreateConnection("Server=localhost:5432;User Id=postgres;Password=test1234");

		protected override string Get123Query => "SELECT regexp_split_to_table('1 2 3', '\\s+')";
	}

	public class SqlCommandExtensionsTests {

		[Fact]
		public void CreateCommand_Returns_Command() {
			var cmd = new NpgsqlCommand();
			var now = DateTimeOffset.Now;
			cmd.WithInput("@Test", now, NpgsqlDbType.Date);
			var p = cmd.Parameters["@Test"];
			Assert.Equal(NpgsqlDbType.Date, p.NpgsqlDbType);
			Assert.Equal(now, p.Value);
		}
	}
}