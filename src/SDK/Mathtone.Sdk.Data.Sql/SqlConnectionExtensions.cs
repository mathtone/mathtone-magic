using Microsoft.SqlServer.Server;
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

	public static class SqlDataReaderExtensions {
		public static async IAsyncEnumerable<RTN> ConsumeAsync<RTN>(this SqlDataReader reader, Func<SqlDataReader, RTN> selector) {
			while (await reader.ReadAsync()) {
				yield return selector(reader);
			}
		}

		public static async IAsyncEnumerable<RTN> ConsumeAsync<RTN>(this SqlDataReader reader, Func<SqlDataReader, Task<RTN>> asyncSelector) {
			while (await reader.ReadAsync()) {
				yield return await asyncSelector(reader);
			}
		}
	}
}