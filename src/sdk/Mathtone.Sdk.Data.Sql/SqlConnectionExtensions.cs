using System.Data;
using System.Data.SqlClient;

namespace Mathtone.Sdk.Data.Sql {
	public static class SqlConnectionExtensions {
		public static SqlCommand CreateCommand(this SqlConnection connection, string commandText, CommandType type = CommandType.Text, int timeout = 30) =>
			 connection.CreateCommand<SqlCommand>(commandText, type, timeout);
	}

	public static class SqlCommandExtensions {

		public static IEnumerable<T> ExecuteResult<T>(this SqlCommand cmd, Func<SqlDataReader, T> selector) =>
			cmd.ExecuteReader().Consume(selector);

		public static IEnumerable<T> ExecuteResult<T>(this SqlCommand cmd, Func<SqlDataReader, Task<T>> selector) =>
			cmd.ExecuteReader().Consume(selector);

		public static IAsyncEnumerable<T> ExecuteResultAsync<T>(this SqlCommand cmd, Func<SqlDataReader, T> selector) =>
			cmd.ExecuteReaderAsync().ConsumeAsync(selector);

		public static IAsyncEnumerable<T> ExecuteResultAsync<T>(this SqlCommand cmd, Func<SqlDataReader, Task<T>> selector) =>
			cmd.ExecuteReaderAsync().ConsumeAsync(selector);
	}
}