using Mathtone.Sdk.Data;
using Npgsql;
using System.Data;

namespace Mathtone.Sdk.Data.Npgsql {

	public static class NpgsqlConnectionExtensions {
		public static NpgsqlCommand CreateCommand(this NpgsqlConnection connection, string commandText, CommandType type = CommandType.Text, int timeout = 30) =>
			 connection.CreateCommand<NpgsqlCommand>(commandText, type, timeout);

		public static NpgsqlCommand TextCommand(this NpgsqlConnection cn, string commandText) => cn.CreateCommand(commandText, CommandType.Text);
		public static NpgsqlCommand ProcCommand(this NpgsqlConnection cn, string commandText) => cn.CreateCommand(commandText, CommandType.StoredProcedure);
		public static NpgsqlCommand TableCommand(this NpgsqlConnection cn, string commandText) => cn.CreateCommand(commandText, CommandType.TableDirect);

		public static async Task<RSLT> ExecuteResult<EXEC, RSLT>(this NpgsqlCommand cmd, Func<NpgsqlCommand, Task<EXEC>> executor, Func<NpgsqlCommand, EXEC, RSLT> selector) =>
			selector(cmd, await executor(cmd));
	}
}