using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Tests {
	public class SqlConnectionExtensionTests {

		string SQL_PWD = "test!1234";
		string PGSQL_PWD = "test1234";

		[Fact]
		public void TestSqlConnection() {
			var cn = new SqlConnection($"Server=localhost;User Id=sa;Password={SQL_PWD}");
			cn.Open();
			cn.Close();
			Assert.True(true);
		}

		[Fact]
		public void TestNpgsqlConnection() {
			var cn = new NpgsqlConnection($"Server=localhost:5432;User Id=postgres;Password={PGSQL_PWD}");
			cn.Open();
			cn.Close();
			Assert.True(true);
		}
	}
}
//docker run --name mathtone-test-pgsql -e POSTGRES_PASSWORD=test1234  -d -p 5432:5432 postgres

//docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=test!1234"  -p 1433:1433 --name mathtone-test-sql --hostname sql1   -d   mcr.microsoft.com/mssql/server:2022-latest
//docker run --name pgsql1 -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=test!1234 -e PGDATA=/var/lib/postgresql/data/pgdata -v /tmp:/var/lib/postgresql/data -p 5432:5432 -it postgres