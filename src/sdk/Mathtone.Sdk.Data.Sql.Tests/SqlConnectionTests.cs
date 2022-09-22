using Mathtone.Sdk.Data.Tests;
using Mathtone.Sdk.Testing.Xunit;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Net.WebSockets;
using Xunit.Abstractions;

namespace Mathtone.Sdk.Data.Sql.Tests {

	public class SqlConnectionTests : DbConnectionTestBase<SqlConnection> {

		public SqlConnectionTests(ITestOutputHelper output) : base(output) { }

		protected override SqlConnection Connect() => new(DB.ConnectionString);
	}
}