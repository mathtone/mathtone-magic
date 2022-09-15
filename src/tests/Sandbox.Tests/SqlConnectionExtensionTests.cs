using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Tests {
	public class SqlConnectionExtensionTests {

		[Fact]
		public void TestConnection() {
			var cn = new SqlConnection("Server=localhost;User Id=sa;Password=test!1234");
			cn.Open();
			cn.Close();
			;
		}
	}
}
