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

	

	public class SqlDataReaderExtensionsTests : DbTestBase<SqlConnection> {

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
			await Connect().UsedAsync(async cn => await cn.CreateCommand(VALUE_QUERY)
				.ExecuteResultAsync(r => (int)r["id"])
				.ToArrayAsync()
			)
		);

		[Fact]
		public async Task ConsumeAsync_2() => Assert.Equal(
			new[] { 1, 2, 3 },
			await Connect().UsedAsync(async cn => await cn.CreateCommand(VALUE_QUERY)
				.ExecuteResultAsync(r => Task.FromResult((int)r["id"]))
				.ToArrayAsync()
			)
		);

		[Fact]
		public async Task Consume_1() => Assert.Equal(
			new[] { 1, 2, 3 },
			await Connect().UsedAsync(cn => cn.CreateCommand(VALUE_QUERY)
				.ExecuteResult(r => (int)r["id"])
				.ToArray()
			)
		);

		[Fact]
		public async Task Consume_2() => Assert.Equal(
			new[] { 1, 2, 3 },
			await Connect().UsedAsync(cn => cn.CreateCommand(VALUE_QUERY)
				.ExecuteResult(r => Task.FromResult((int)r["id"]))
				.ToArray()
			)
		);
	}
}