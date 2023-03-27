using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mathtone.Sdk.Data.Sql;
using System.Data;
using Mathtone.Sdk.Data;

namespace Sandbox.Tests {
	//public class SqlConnectionExtensionTests {

	//	string SQL_PWD = "test!1234";
	//	string PGSQL_PWD = "test1234";

	//	public void CreateCommand() {
	//		var rslt = new SqlConnection($"Server=localhost;User Id=sa;Password={SQL_PWD}")
	//			.Used(cn => cn
	//				.CreateCommand("SELECT @Value as MyValue")
	//				.WithInput("@Value", 1000, SqlDbType.Int)
	//				.ExecuteScalar()
	//			);
	//		Assert.Equal(1000, rslt);
	//	}

	//	[Fact]
	//	public void TestNpgsqlConnection() {
	//		var rslt = new NpgsqlConnection($"Server=localhost;User Id=sa;Password={SQL_PWD}")
	//			.Used(cn => cn
	//				.CreateCommand("SELECT @Value as MyValue")
	//				.WithInput("@Value", 1000, SqlDbType.Int)
	//				.ExecuteScalar()
	//			);
	//		Assert.Equal(1000, rslt);
	//	}
	//}
}
//docker run --name mathtone-test-pgsql -e POSTGRES_PASSWORD=test1234  -d -p 5432:5432 postgres
//docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=test!1234"  -p 1433:1433 --name mathtone-test-sql --hostname sql1   -d   mcr.microsoft.com/mssql/server:2022-latest
//docker run --name pgsql1 -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=test!1234 -e PGDATA=/var/lib/postgresql/data/pgdata -v /tmp:/var/lib/postgresql/data -p 5432:5432 -it postgres