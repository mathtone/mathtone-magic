using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using Xunit.Abstractions;

namespace Mathtone.Sdk.Data.Tests {
	public abstract class DbDataReaderExtensionsTest<CN, RDR> : IDbDataReaderExtensionsTest<CN, RDR>

		where CN : DbConnection
		where RDR : DbDataReader {

		protected DbDataReaderExtensionsTest(ITestOutputHelper output) : base(output) {
		}

		[Fact]
		public async Task ConsumeAsync_Result() => Assert.Equal(
			new[] { "A", "B", "C" },
			await Connect().Used(cn => cn
				.CreateCommand<DbCommand>(Queries.TestQuery)
				.ExecuteReader()
				.ConsumeAsync(r => r["value"].ToString())
				.ToArrayAsync()
			)
		);

		[Fact]
		public async Task ProcessAsync_Result() {
			var i = 0;
			await using var cn = Connect();
			await cn.CreateCommand<DbCommand>(Queries.TestQuery)
				.ExecuteReader()
				.ProcessAsync(r => r["value"].ToString(), r => i++);

			Assert.Equal(3, i);
		}
	}
}