using System.Data;
using System.Data.Common;
using Moq;
namespace Mathtone.Sdk.Data.Tests.Support {
	public static class MockDbCommand {
		public static Mock<DbCommand> CreateMockDbCommand() {
			var mockDbCommand = new Mock<DbCommand>();
			mockDbCommand.SetupProperty(x => x.CommandText);
			mockDbCommand.SetupProperty(x => x.CommandType);
			return mockDbCommand;
		}
	}

	public class TestDbCommand : DbCommand {
		public override string CommandText { get; set; }
		public override int CommandTimeout { get; set; }
		public override CommandType CommandType { get; set; }
		public override bool DesignTimeVisible { get; set; }
		public override UpdateRowSource UpdatedRowSource { get; set; }
		protected override DbConnection? DbConnection { get; set; }
		protected override DbParameterCollection DbParameterCollection { get; }
		protected override DbTransaction? DbTransaction { get; set; }

		public override void Cancel() {
			throw new NotImplementedException();
		}

		public override int ExecuteNonQuery() {
			throw new NotImplementedException();
		}

		public override object? ExecuteScalar() {
			throw new NotImplementedException();
		}

		public override Task<object?> ExecuteScalarAsync(CancellationToken cancellationToken) => Task.FromResult((object?)"TEST");

		public override void Prepare() {
			throw new NotImplementedException();
		}

		protected override DbParameter CreateDbParameter() {
			throw new NotImplementedException();
		}

		protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior) {
			throw new NotImplementedException();
		}
	}
}