using Mathtone.Sdk.Data.Tests;
using System.Data;
using System.Data.SqlClient;
using Xunit.Abstractions;

namespace Mathtone.Sdk.Data.Sql.Tests {
	
	internal static class DB {
		public static readonly string ConnectionString = $"Server=localhost;User Id=sa;Password={Environment.GetEnvironmentVariable("SQL_TEST_PWD")}";
	}

	public class SqlCommandTests : DbCommandTestBase<SqlConnection, SqlCommand> {

		public SqlCommandTests(ITestOutputHelper output) : base(output) { }

		protected override SqlConnection Connect() => new(DB.ConnectionString);

		[Fact]
		public void WithInput_SqlDbType() => Assert.Equal(
			1,
			new SqlCommand().WithInput("@id", 1, SqlDbType.Int).Parameters["@id"].Value
		);

		[Fact]
		public void WithOutput_SqlDbType() => Assert.Equal(
			ParameterDirection.Output,
			new SqlCommand().WithOutput("@id", SqlDbType.Int).Parameters["@id"].Direction
		);

		[Fact]
		public void WithInputOutput_SqlDbType() => Assert.Equal(
			ParameterDirection.InputOutput,
			new SqlCommand().WithInputOutput("@id", 1,SqlDbType.Int).Parameters["@id"].Direction
		);
	}
}