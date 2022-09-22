using Mathtone.Sdk.Data.Tests;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Mathtone.Sdk.Data.Npgsql.Tests {
	public class NpgsqlConnectionTests : IDbConnectionTestBase<NpgsqlConnection> {

		public NpgsqlConnectionTests(ITestOutputHelper output) : base(output) { }

		protected override NpgsqlConnection Connect() => new(DB.ConnectionString);
	}
}