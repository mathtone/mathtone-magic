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
}