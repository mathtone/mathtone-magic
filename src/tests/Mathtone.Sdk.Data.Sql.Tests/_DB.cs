using Mathtone.Sdk.Data.Tests;
using Mathtone.Sdk.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathtone.Sdk.Data.Sql.Tests {

	internal static class DB {
		//public static readonly string ConnectionString = $"Server=localhost;User Id=sa;Password={Environment.GetEnvironmentVariable("SQL_TEST_PWD")}";
		public static readonly string ConnectionString = $"Server=localhost;User Id=sa;Password=test!1234";
	}

	[ResourcePath("Queries")]
	public class SqlTestQueries : TestQueries { }
}