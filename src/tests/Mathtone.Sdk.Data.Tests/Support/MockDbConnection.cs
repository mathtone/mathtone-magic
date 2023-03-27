using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
namespace Mathtone.Sdk.Data.Tests.Support {

	//add settable CommandText and commandtype properties to the Mock<IDbCommand>() object
	public static class MockDbConnection {
		public static Mock<IDbConnection> CreateMockDbConnection() {
			var mockDbConnection = new Mock<IDbConnection>();
			mockDbConnection.Setup(x => x.CreateCommand()).Returns(MockDbCommand.CreateMockDbCommand().Object);
			return mockDbConnection;
		}
	}
}