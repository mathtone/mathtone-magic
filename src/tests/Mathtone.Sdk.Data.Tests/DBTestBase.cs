﻿using Mathtone.Sdk.Testing.Xunit;
using Mathtone.Sdk.Utilities.Resources;
using System.Data;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Mathtone.Sdk.Data.Tests {

	public abstract class TestQueries : MaterializedLocalResources {
		public readonly string TestQuery;
	}

	public abstract class DBTestBase<CN> : XunitTestBase {

		protected DBTestBase(ITestOutputHelper output) :
			base(output) { }

		protected abstract CN Connect();
	}

	public abstract class IDbDataReaderExtensionsTest<CN, RDR> : DBTestBase<CN>

		where CN : IDbConnection
		where RDR : IDataReader {

		protected IDbDataReaderExtensionsTest(ITestOutputHelper output) : base(output) {
		}

		[Fact]
		public virtual void Consume() => Assert.Equal(
			new[] { "A", "B", "C" },
			Connect().Used(
				cn => cn.CreateCommand(Queries.TestQuery, CommandType.Text)
					.ExecuteReader()
					.Consume(r => r["value"].ToString())
					.ToArray()
				)
		);

		protected abstract TestQueries Queries { get; }
	}

	//public abstract class DbTestBase<CN> where CN : IDbConnection, new() {
	//	protected abstract string ConnectionString { get; }
	//	protected CN Connect() => new() { ConnectionString = ConnectionString };
	//}

	//public abstract class DbConnectionTestBase<CN> : DbTestBase<CN>
	//	where CN : DbConnection, new() {

	//	[Fact]
	//	public async Task MakeConnection() {
	//		using var cn = Connect();
	//		await cn.OpenAsync();
	//		Assert.Equal(ConnectionState.Open, cn.State);
	//		await cn.CloseAsync();
	//	}

	//	[Fact]
	//	public void CreateCommand_1() {
	//		Assert.NotNull(Connect().CreateCommand());
	//	}

	//	[Fact]
	//	public void CreateCommand_2() {
	//		var cmd = Connect().CreateCommand("TEST");
	//		Assert.Equal("TEST", cmd.CommandText);
	//	}
	//}


	//public abstract class IDbConnectionExtensionsTests<CN, CMD>
	//	where CN : IDbConnection
	//	where CMD : IDbCommand {

	//	abstract protected CN CreateConnection();
	//	abstract protected CN CreateConnection(string connectionString);
	//	abstract protected CMD CreateCommand(string commandText);

	//	[Fact]
	//	public virtual void WithTemplate_Replace_Text() {
	//		var cmd = CreateCommand("THIS IS $DEFINITELY$ A TEST")
	//			.WithTemplate("$DEFINITELY$", "PROBABLY");

	//		Assert.Equal("THIS IS PROBABLY A TEST", cmd.CommandText);

	//	}

	//	[Theory]
	//	[InlineData(true, "THIS IS PROB''ABLY A TEST")]
	//	[InlineData(false, "THIS IS PROB'ABLY A TEST")]
	//	public virtual void WithTemplate_Escape_Text(bool escape, string expected) {
	//		var cmd = CreateCommand("THIS IS $DEFINITELY$ A TEST")
	//			.WithTemplate("$DEFINITELY$", "PROB'ABLY", escape);

	//		Assert.Equal(expected, cmd.CommandText);

	//	}

	//	[Fact]
	//	public virtual void WithInput() {
	//		var cmd = CreateCommand("THIS IS A @TEST")
	//			.WithInput("@TEST1", 1)
	//			.WithInput("@TEST2", 2)
	//			.WithInput("@TEST3", 3);

	//		Assert.Equal(3, cmd.Parameters.Count);

	//	}

	//	[Fact]
	//	public virtual void WithOutput() {
	//		var cmd = CreateCommand("THIS IS A @TEST")
	//			.WithOutput("@TEST1", 1);
	//		Assert.Equal(1, cmd.Parameters.Count);
	//	}

	//	[Fact]
	//	public virtual void WithInputOutput() {
	//		var cmd = CreateCommand("THIS IS A @TEST")
	//			.WithInputOutput("@TEST1");
	//		Assert.Equal(1, cmd.Parameters.Count);
	//	}

	//	[Fact]
	//	public virtual void Excute_NoResult() {
	//		var rslt = 0;
	//		CreateCommand("THIS IS A @TEST")
	//			.Execute(cmd => rslt = 2);

	//		Assert.Equal(2, rslt);
	//	}

	//	[Fact]
	//	public virtual void Execute_Result() {
	//		var rslt = CreateCommand("THIS IS A @TEST")
	//			.ExecuteResult(cmd => 2, rslt => rslt * 2);

	//		Assert.Equal(4, rslt);
	//	}

	//	[Fact]
	//	public virtual void Execute_Result_2() {
	//		var rslt = CreateCommand("THIS IS A @TEST")
	//			.ExecuteResult(cmd => 2, (cmd, rslt) => cmd.CommandType);

	//		Assert.Equal(CommandType.Text, rslt);
	//	}

	//	[Fact]
	//	public virtual void ExecuteReader() {
	//		var cmd = CreateCommand(GetDateQuery);
	//		cmd.Connection = CreateConnection();
	//		using (cmd.Connection) {
	//			cmd.Connection.Open();
	//			var rslt = cmd
	//				.ExecuteRead(r => r);

	//			Assert.True(rslt.GetType().IsAssignableTo(typeof(IDataReader)));
	//		}
	//	}

	//	[Fact]
	//	public virtual void ExecuteReader_2() {
	//		var cmd = CreateCommand(GetDateQuery);
	//		cmd.Connection = CreateConnection();
	//		using (cmd.Connection) {
	//			cmd.Connection.Open();
	//			var rslt = cmd
	//				.ExecuteRead((cmd,r) => cmd);

	//			Assert.Equal(cmd,rslt);
	//		}
	//	}

	//	[Fact]
	//	public virtual void ExecuteReadSingle() {
	//		var cmd = CreateCommand(GetDateQuery);
	//		cmd.Connection = CreateConnection();
	//		using (cmd.Connection) {
	//			cmd.Connection.Open();
	//			var rslt = cmd
	//				.ExecuteReadSingle(r => r.Field("Value"));
	//			;
	//			Assert.NotEqual(DateTime.MinValue,DateTime.Parse(rslt));
	//		}
	//	}

	//	[Fact]
	//	public virtual void ExecuteReadToArray() {
	//		var cmd = CreateCommand(Get123Query);
	//		cmd.Connection = CreateConnection();
	//		using (cmd.Connection) {
	//			cmd.Connection.Open();
	//			var rslt = cmd.ExecuteReader().ToArray(r=>r.Field<int>(0));
	//			Assert.Equal(new[] {1,2,3},rslt);
	//		}
	//	}

	//	[Fact]
	//	public virtual void ExecuteReadToArray2() {
	//		var cmd = CreateCommand(Get123Query);
	//		cmd.Connection = CreateConnection();
	//		using (cmd.Connection) {
	//			cmd.Connection.Open();
	//			var rslt = cmd.ExecuteReader().ToArray(r => r.Field(0));
	//			Assert.Equal(new[] { "1", "2", "3" }, rslt);
	//		}
	//	}

	//	protected abstract string GetDateQuery { get; }
	//	protected abstract string Get123Query { get; }
	//}
}