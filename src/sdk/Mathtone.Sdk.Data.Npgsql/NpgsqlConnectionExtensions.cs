using Mathtone.Sdk.Data;
using System.Data;
using Npgsql;

namespace Mathtone.Sdk.Data.Npgsql {

	public static class NpgsqlConnectionExtensions {
		public static NpgsqlCommand CreateCommand(this NpgsqlConnection connection, string commandText, CommandType type = CommandType.Text, int timeout = 30) =>
			 connection.CreateCommand<NpgsqlCommand>(commandText, type, timeout);
	}
}