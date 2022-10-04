using Mathtone.Sdk.Data.Tests;
using Npgsql;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Xunit.Abstractions;

namespace Mathtone.Sdk.Data.Npgsql.Tests {

	public class NpgsqlCommandTests : DbCommandTestBase<NpgsqlConnection, SqlCommand> {

		public NpgsqlCommandTests(ITestOutputHelper output) : base(output) { }

		protected override NpgsqlConnection Connect() => new(DB.ConnectionString);
	}
}