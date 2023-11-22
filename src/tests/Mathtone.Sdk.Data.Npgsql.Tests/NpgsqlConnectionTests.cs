using Mathtone.Sdk.Data.Tests;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Mathtone.Sdk.Data.Npgsql.Tests {
	public class NpgsqlConnectionTests : IDbConnectionTestBase<NpgsqlConnection> {

		public NpgsqlConnectionTests(ITestOutputHelper output) : base(output) { }

		protected override NpgsqlConnection Connect() => new(DB.ConnectionString);

		[Fact]
		public void TextCommand() =>
			Assert.Equal(CommandType.Text, new NpgsqlConnection().TextCommand("TEST").CommandType);

		[Fact]
		public void ProcCommand() =>
			Assert.Equal(CommandType.StoredProcedure, new NpgsqlConnection().ProcCommand("TEST").CommandType);
		
		[Fact]
		public void TableCommand() =>
			Assert.Equal(CommandType.TableDirect, new NpgsqlConnection().TableCommand("TEST").CommandType);

		[Fact]
		public async Task UsedTestAsync() {
			var cn = Connect();
			var r = await cn.UsedAsync(async cn=> {
				Assert.Equal(ConnectionState.Open, cn.State);
				return (await cn.TextCommand("SELECT 'HELLO';").ExecuteReaderAsync()).ConsumeAsync(r => r[0].ToString()).ToArrayAsync();
			});
			
			Assert.Equal(ConnectionState.Closed, cn.State);
		}

	}
}