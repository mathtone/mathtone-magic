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
	public static class DB {
		public static readonly Queries SQL = new();
		public static readonly string ConnectionString = $"Server=localhost;User Id=sa;Password={Environment.GetEnvironmentVariable("SQL_TEST_PWD")}";
	}

	public class SqlCommandTests : DbTestBase<SqlConnection> {

		protected override string ConnectionString => DB.ConnectionString;

		[Fact]
		public void WithInput_1() => Assert.Equal(
			2,
			new SqlCommand().WithInput("@id", 2).Parameters["@id"].Value
		);

		[Fact]
		public void WithInput_2() => Assert.Equal(
			2,
			new SqlCommand().WithInput("@id", 2, SqlDbType.Int).Parameters["@id"].Value
		);

		[Fact]
		public void WithOutput_1() => Assert.Equal(
			ParameterDirection.Output,
			new SqlCommand().WithOutput("@out").Parameters["@out"].Direction
		);

		[Fact]
		public void WithOutput_2() => Assert.Equal(
			ParameterDirection.Output,
			new SqlCommand().WithOutput("@out", SqlDbType.Int).Parameters["@out"].Direction
		);

		[Fact]
		public void WithInputOutput() {
			var cmd = new SqlCommand().WithInputOutput("@inout", 1);
			var p = cmd.Parameters["@inout"];
			Assert.Equal(ParameterDirection.InputOutput, p.Direction);
			Assert.Equal(1, p.Value);
		}

		[
			Theory,
			InlineData("WHATEVER ORDER BY $ORDERBY$", "$ORDERBY$", "VALUE DESC", false, "WHATEVER ORDER BY VALUE DESC"),
			InlineData("WHATEVER ORDER BY $ORDERBY$", "$ORDERBY$", "VALUE DE'SC", true, "WHATEVER ORDER BY VALUE DE''SC"),
			InlineData("WHATEVER ORDER BY $ORDERBY$", "$ORDERBY$", "VALUE DE'SC", false, "WHATEVER ORDER BY VALUE DE'SC")
		]
		public void WithTemplate(string input, string pName, string pValue, bool escape, string expect) => Assert.Equal(
			expect,
			new SqlCommand(input).WithTemplate(pName, pValue, escape).CommandText
		);
	}

	public class SqlDataReaderExtensionsTests : DbTestBase<SqlConnection> {


		protected override string ConnectionString => DB.ConnectionString;

		[Fact]
		public async Task ConsumeAsync_1() => Assert.Equal(
			new[] { 1, 2, 3 },
			await Connect().UsedAsync(async cn => await cn.CreateCommand(DB.SQL.TestQuery)
				.ExecuteResultAsync(r => (int)r["id"])
				.ToArrayAsync()
			)
		);

		[Fact]
		public async Task ConsumeAsync_2() => Assert.Equal(
			new[] { 1, 2, 3 },
			await Connect().UsedAsync(async cn => await cn.CreateCommand(DB.SQL.TestQuery)
				.ExecuteResultAsync(r => Task.FromResult((int)r["id"]))
				.ToArrayAsync()
			)
		);

		[Fact]
		public async Task Consume_1() => Assert.Equal(
			new[] { 1, 2, 3 },
			await Connect().UsedAsync(cn => cn.CreateCommand(DB.SQL.TestQuery)
				.ExecuteResult(r => (int)r["id"])
				.ToArray()
			)
		);

		[Fact]
		public async Task Consume_2() => Assert.Equal(
			new[] { 1, 2, 3 },
			await Connect().UsedAsync(cn => cn.CreateCommand(DB.SQL.TestQuery)
				.ExecuteResult(r => Task.FromResult((int)r["id"]))
				.ToArray()
			)
		);
	}
}