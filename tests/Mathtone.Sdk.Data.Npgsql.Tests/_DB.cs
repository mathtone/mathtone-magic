using Mathtone.Sdk.Data.Tests;
using Mathtone.Sdk.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathtone.Sdk.Data.Npgsql.Tests {

	internal static class DB {
		public static readonly string ConnectionString = $"Server=localhost;User Id=postgres;Password={Environment.GetEnvironmentVariable("PGSQL_TEST_PWD")}";
	}

	[ResourcePath("Queries")]
	public class NpgsqlTestQueries : TestQueries { }
}