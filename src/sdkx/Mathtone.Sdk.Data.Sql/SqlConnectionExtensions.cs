using System.Data;
using System.Data.SqlClient;

namespace Mathtone.Sdk.Data.Sql {

	public static class SqlConnectionExtensions {
		public static SqlCommand CreateCommand(this SqlConnection connection, string commandText, CommandType type = CommandType.Text, int timeout = 30) =>
			 connection.CreateCommand<SqlCommand>(commandText, type, timeout);
	}
}