using Mathtone.Sdk.Data.Tests;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Net;
using System.Reflection.PortableExecutable;
using Xunit.Abstractions;

namespace Mathtone.Sdk.Data.Sql.Tests {

	public static class TestDb {
		public static readonly string ConnectionString = $"Server=localhost;User Id=sa;Password={Environment.GetEnvironmentVariable("SQL_TEST_PWD")}";
		public static SqlConnection Connect() => new(ConnectionString);
	}

	public class SqlConnectionTest : DbConnectionTestBase<SqlConnection> {
		protected override string ConnectionString => TestDb.ConnectionString;
	}

	public class DbDataReaderExtensionsTests {

		const string VALUE_QUERY =
		@"
			SELECT 1 id,'A' value
			UNION ALL SELECT 2,'B'
			UNION ALL SELECT 3,'C'
		";

		[Fact]
		public async Task ConsumeAsync_1() {
			using var cmd = TestDb.Connect().CreateCommand<SqlCommand>(VALUE_QUERY);
			await cmd.Connection.OpenAsync();
			var rslt = await cmd.ExecuteReaderAsync()
				.ConsumeAsync(r => (int)r["id"])
				.ToArrayAsync();
			await cmd.Connection.CloseAsync();
			Assert.Equal(new[] { 1, 2, 3 }, rslt);
		}

		[Fact]
		public async Task ConsumeAsync_2() {
			using var cmd = TestDb.Connect().CreateCommand<SqlCommand>(VALUE_QUERY);
			await cmd.Connection.OpenAsync();
			var rslt = await cmd.ExecuteReaderAsync()
				.ConsumeAsync(r => Task.FromResult((int)r["id"]))
				.ToArrayAsync();
			await cmd.Connection.CloseAsync();
			Assert.Equal(new[] { 1, 2, 3 }, rslt);
		}

		[Fact]
		public async Task Consume_1() {
			using var cmd = TestDb.Connect().CreateCommand<SqlCommand>(VALUE_QUERY);
			await cmd.Connection.OpenAsync();
			var rslt = (await cmd.ExecuteReaderAsync())
				.Consume(r => (int)r["id"])
				.ToArray();
			await cmd.Connection.CloseAsync();
			Assert.Equal(new[] { 1, 2, 3 }, rslt);
		}

		[Fact]
		public async Task Consume_2() {
			using var cmd = TestDb.Connect().CreateCommand<SqlCommand>(VALUE_QUERY);
			await cmd.Connection.OpenAsync();
			var rslt = (await cmd.ExecuteReaderAsync())
				.Consume(r => Task.FromResult((int)r["id"]))
				.ToArray();
			await cmd.Connection.CloseAsync();
			Assert.Equal(new[] { 1, 2, 3 }, rslt);
		}
	}
}