using Mathtone.Sdk.Common;
using Mathtone.Sdk.Common.Extensions;
using Mathtone.Sdk.Patterns;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Security;
using Xunit.Abstractions;

namespace Mathtone.Sdk.Data.Tests {
	[SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Because interface")]
	public abstract class IDbConnectionTestBase<CN> : DBTestBase<CN> where CN : IDbConnection {

		protected IDbConnectionTestBase(ITestOutputHelper output) :
			base(output) { }

		[Fact]
		public virtual void CreateCommand() {
			var cn = Connect();
			var cmd = cn.CreateCommand("TEST");
			Assert.Equal("TEST", cmd.CommandText);
			Assert.Equal(CommandType.StoredProcedure, cmd.CommandType);
		}

		[Fact]
		public virtual void Used() {
			var cn = Connect();
			cn.Used(c => Assert.Equal(ConnectionState.Open, c.State));
			Assert.Equal(ConnectionState.Closed, cn.State);
		}
	}

	public class DictionaryRepositoryTests {

		[Fact]
		public void TestRepo() {
			var id = 3;
			var repo = new[] {
				new { Id = 1, Val = "One" },
				new { Id = 2, Val = "Two" },
				new { Id = 3, Val = "Three" }
			}.ToRepo(i => i.Id);

			Assert.Equal("Two", repo.Read(2).Val);
			Assert.Equal(4, repo.Create(new { Id = 4, Val = "Four" }));
		}


	}
	//var repo = new DictionaryRepository<int, object> = new(i) {
	//	{ 1, new{Id=1,Val="One"} },
	//	{ 1, new{Id=1,Val="One"} },
	//	{ 1, new{Id=1,Val="One"} }
	//}
	//Assert.Equal(repo.Create(new ))		}


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