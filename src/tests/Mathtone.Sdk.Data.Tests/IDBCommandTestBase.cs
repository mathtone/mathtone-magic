using System.Data;
using System.Diagnostics.CodeAnalysis;
using Xunit.Abstractions;

namespace Mathtone.Sdk.Data.Tests {
	[SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Because interface")]
	public abstract class IDBCommandTestBase<CN, CMD> : DBTestBase<CN>
		where CN : IDbConnection
		where CMD : IDbCommand {

		protected IDBCommandTestBase(ITestOutputHelper output) :
			base(output) { }


		[Theory]
		[InlineData(true, "THIS IS PROB''ABLY A TEST")]
		[InlineData(false, "THIS IS PROB'ABLY A TEST")]
		public virtual void WithTemplate_Escape_Text(bool escape, string expected) {
			var cmd = Connect().CreateCommand("THIS IS $DEFINITELY$ A TEST")
				.WithTemplate("$DEFINITELY$", "PROB'ABLY", escape);

			Assert.Equal(expected, cmd.CommandText);

		}

		[Fact]
		public virtual void WithInput() {
			var p = Connect().CreateCommand().WithInput("@id", 2).Parameters["@id"];
			Assert.NotNull(p);
		}

		[Fact]
		public virtual void WithInputOutput() {
			var p = Connect().CreateCommand().WithInputOutput("@id", 2).Parameters["@id"];
			Assert.NotNull(p.ToString());
		}
	}
}