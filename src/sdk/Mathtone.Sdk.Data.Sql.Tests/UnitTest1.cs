using Mathtone.Sdk.Data.Tests;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Mathtone.Sdk.Data.Sql.Tests {
	

	public class SqlConnectionTest : DbConnectionTestBase<SqlConnection>{
		
		protected override string ConnectionString { get; } = $"Server=localhost,5150;User Id=sa;Password={Environment.GetEnvironmentVariable("SQL_TEST_PWD")}";
	}
}