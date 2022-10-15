using Mathtone.Sdk.Data.Tests;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using System.Data.Common;
using Xunit.Abstractions;

namespace Mathtone.Sdk.Data.Npgsql.Tests {

	public class NpgsqlCommandTests : DbCommandTestBase<NpgsqlConnection, NpgsqlCommand>  {

		public NpgsqlCommandTests(ITestOutputHelper output) : base(output) { }

		protected override NpgsqlConnection Connect() => new(DB.ConnectionString);

		[Fact]
		public void WithInput_SqlDbType() {
			var p = Connect().CreateCommand("").WithInput("@id", 1, NpgsqlDbType.Integer).Parameters["@id"];
			Assert.Equal(1, p.Value);
			Assert.Equal(ParameterDirection.Input, p.Direction);
		}

	
	}
}