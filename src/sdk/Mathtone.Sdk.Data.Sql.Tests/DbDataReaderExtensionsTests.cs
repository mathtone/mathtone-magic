using Mathtone.Sdk.Data.Tests;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net;
using System.Reflection.PortableExecutable;
using Xunit.Abstractions;

namespace Mathtone.Sdk.Data.Sql.Tests {

	//public interface IDbConnector<CN> where CN : IDbConnection {
	//	CN Connect();
	//}

	//public class SqlDbConnector : IDbConnector<SqlConnection> {
	//	readonly string _connectionString;

	//	public SqlDbConnector(string connectionString) => _connectionString = connectionString;

	//	public SqlConnection Connect() => new(_connectionString);
	//}

	public abstract class DbTestbase<CN> where CN : IDbConnection, new() {
		protected abstract string ConnectionString { get; }
		protected CN Connect() => new() { ConnectionString = ConnectionString };
	}

	public abstract class DbConnectionTestBase<CN> : DbTestbase<CN>
		where CN : DbConnection, new() {

		[Fact]
		public async Task MakeConnection() {
			using var cn = Connect();
			await cn.OpenAsync();
			Assert.Equal(ConnectionState.Open, cn.State);
			await cn.CloseAsync();
		}
	}

	public class SqlConnectionTest : DbConnectionTestBase<SqlConnection> {
		protected override string ConnectionString => $"Server=localhost;User Id=sa;Password={Environment.GetEnvironmentVariable("SQL_TEST_PWD")}";
	}

	public static class SqlConnectionExtensions {
		public static SqlCommand CreateCommand(this SqlConnection connection, string commandText, CommandType type = CommandType.Text, int timeout = 30) =>
			 connection.CreateCommand<SqlCommand>(commandText, type, timeout);
	}

	public class DbDataReaderExtensionsTests : DbTestbase<SqlConnection> {

		protected override string ConnectionString { get; } = $"Server=localhost;User Id=sa;Password={Environment.GetEnvironmentVariable("SQL_TEST_PWD")}";

		const string VALUE_QUERY =
		@"
			SELECT 1 id,'A' value
			UNION ALL SELECT 2,'B'
			UNION ALL SELECT 3,'C'
		";

		[Fact]
		public async Task ConsumeAsync_1() => Assert.Equal(
			new[] { 1, 2, 3 },
			await Connect().UsedAsync(async cn =>
				await cn.CreateCommand(VALUE_QUERY)
					.ExecuteReaderAsync()
					.ConsumeAsync(r => (int)r["id"])
					.ToArrayAsync()
			)
		);

		[Fact]

		public async Task ConsumeAsync_2() => Assert.Equal(
			new[] { 1, 2, 3 },
			await Connect().UsedAsync(async cn =>
				await cn.CreateCommand(VALUE_QUERY)
					.ExecuteReaderAsync()
					.ConsumeAsync(r => Task.FromResult((int)r["id"]))
					.ToArrayAsync()
			)
		);

		[Fact]
		public async Task Consume_1() => Assert.Equal(
			new[] { 1, 2, 3 },
			await Connect().UsedAsync(cn =>
				cn.CreateCommand(VALUE_QUERY)
					.ExecuteReader()
					.Consume(r => (int)r["id"])
					.ToArray()
			)
		);

		[Fact]
		public async Task Consume_2() => Assert.Equal(
			new[] { 1, 2, 3 },
			await Connect().UsedAsync(cn =>
				cn.CreateCommand(VALUE_QUERY)
					.ExecuteReader()
					.Consume(r => Task.FromResult((int)r["id"]))
					.ToArray()
			)
		);
	}
}