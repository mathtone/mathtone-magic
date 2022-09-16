using System.Data.SqlClient;
using System.Data;

namespace Mathtone.Sdk.Data.Sql.Tests {
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
}