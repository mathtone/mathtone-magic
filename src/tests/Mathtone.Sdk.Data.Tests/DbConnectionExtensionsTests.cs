using Mathtone.Sdk.Common;
using Mathtone.Sdk.Common.Extensions;
using Mathtone.Sdk.Data.Tests.Support;
using Mathtone.Sdk.Patterns;
using System.Data;
using System.Security;
using System.Data.Common;
using Moq;

namespace Mathtone.Sdk.Data.Tests {

	public class DbConnectionExtensionsTests {

		readonly IDbConnection _connection = MockDbConnection.CreateMockDbConnection().Object;

		[Fact]
		public void TextCommand() => Assert.Equal(CommandType.Text, _connection.TextCommand<IDbConnection, IDbCommand>("TEST").CommandType);
		[Fact]
		public void ProcCommand() => Assert.Equal(CommandType.StoredProcedure, _connection.ProcCommand<IDbConnection, IDbCommand>("TEST").CommandType);
		[Fact]
		public void TableCommand() => Assert.Equal(CommandType.TableDirect, _connection.TableCommand<IDbConnection, IDbCommand>("TEST").CommandType);
	}
}