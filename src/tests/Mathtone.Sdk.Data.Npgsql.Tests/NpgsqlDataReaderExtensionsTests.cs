//using Mathtone.Sdk.Data.Tests;
using Mathtone.Sdk.Data.Tests;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net;
using System.Reflection.PortableExecutable;
using Xunit.Abstractions;

namespace Mathtone.Sdk.Data.Npgsql.Tests {

	//public class NpgsqlDataReaderExtensionsTests : IDbDataReaderExtensionsTest<NpgsqlConnection, SqlDataReader> {

	//	public NpgsqlDataReaderExtensionsTests(ITestOutputHelper output) : base(output) { }

	//	protected override NpgsqlConnection Connect() => new(DB.ConnectionString);

	//	protected override TestQueries Queries { get; } = new NpgsqlTestQueries();

	//	//[Fact]
	//	//public async Task ConsumeAsync_Task_Result() {
	//	//	var cmd = Connect().CreateCommand(Queries.TestQuery);
	//	//	await cmd.Connection.OpenAsync();
	//	//	var rslt = await cmd.ExecuteReaderAsync()
	//	//		.ConsumeAsync(r => r["value"].ToString())
	//	//		.ToArrayAsync();
	//	//	Assert.Equal(new[] { "A", "B", "C" }, rslt);
	//	//}
	//}
}