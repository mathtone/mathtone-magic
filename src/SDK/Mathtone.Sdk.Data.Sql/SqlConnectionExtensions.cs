using System.Data;
using System.Data.SqlClient;

namespace Mathtone.Sdk.Data.Sql {
	public static class SqlConnectionExtensions {
		public static SqlCommand CreateCommand(this SqlConnection connection, string commandText, CommandType type = CommandType.Text) {
			var rtn = connection.CreateCommand();
			rtn.CommandText = commandText;
			rtn.CommandType = type;
			return rtn;
		}
	}
}