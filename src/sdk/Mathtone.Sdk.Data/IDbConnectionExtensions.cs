using System.Data;
using System.Data.Common;

namespace Mathtone.Sdk.Data {
	public static class IDbConnectionExtensions {

		public static CMD CreateCommand<CMD>(this IDbConnection connection)
			where CMD : DbCommand => (CMD)connection.CreateCommand();

		public static CMD CreateCommand<CMD>(this IDbConnection connection, string commandText, CommandType type = CommandType.Text, int timeout = 30 )
			where CMD : DbCommand {
			var rtn = connection.CreateCommand<CMD>();
			rtn.CommandText = commandText;
			rtn.CommandType = type;
			rtn.CommandTimeout = timeout;
			return rtn;
		}
	}
}