using System.Data;
using System.Data.SqlClient;

namespace Mathtone.Sdk.Data.Sql {
	public static class SqlCommandExtensions {

		public static SqlCommand WithParameter<T>(this SqlCommand command, string name, T value, ParameterDirection direction, SqlDbType type, int size = default) {
			var p = command.CreateParameter();
			p.ParameterName = name;
			p.Value = value == null ? DBNull.Value : value,
			p.Direction = direction;
			p.Size = size;
			p.SqlDbType = type;
			return command.WithParameter(p);
		}

		public static SqlCommand WithInput<T>(this SqlCommand command, string name, T value, SqlDbType type, int size = default) =>
			command.WithParameter(name, value, ParameterDirection.Input, type, size);

		public static SqlCommand WithOutput(this SqlCommand command, string name, SqlDbType type = default, int size = default) =>
			command.WithOutput<object>(name, type, size);

		public static SqlCommand WithOutput<T>(this SqlCommand command, string name, SqlDbType type = default, int size = default) =>
			command.WithParameter(name, default(T), ParameterDirection.Output, type, size);

		public static SqlCommand WithInputOutput<T>(this SqlCommand command, string name, T value, SqlDbType type, int size = default) =>
			command.WithParameter(name, value, ParameterDirection.InputOutput, type, size);

		public static SqlCommand TextCommand(this SqlConnection cn, string commandText) => cn.CreateCommand(commandText, CommandType.Text);
		public static SqlCommand ProcCommand(this SqlConnection cn, string commandText) => cn.CreateCommand(commandText, CommandType.StoredProcedure);
		public static SqlCommand TableCommand(this SqlConnection cn, string commandText) => cn.CreateCommand(commandText, CommandType.TableDirect);

		public static async Task<RSLT> ExecuteResult<EXEC, RSLT>(this SqlCommand cmd, Func<SqlCommand, Task<EXEC>> executor, Func<SqlCommand, EXEC, RSLT> selector) =>
			selector(cmd, await executor(cmd));
	}
}