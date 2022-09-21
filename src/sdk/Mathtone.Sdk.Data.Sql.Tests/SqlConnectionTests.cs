using Mathtone.Sdk.Data.Tests;
using System.Data.SqlClient;

namespace Mathtone.Sdk.Data.Sql.Tests {
	public class SqlConnectionTests : DbConnectionTestBase<SqlConnection> {
		protected override string ConnectionString { get; } = $"Server=localhost;User Id=sa;Password={Environment.GetEnvironmentVariable("SQL_TEST_PWD")}";
	}
}