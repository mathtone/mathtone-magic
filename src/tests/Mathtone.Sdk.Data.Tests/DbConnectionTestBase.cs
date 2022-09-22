using System.Data.Common;
using System.Security.AccessControl;
using Mathtone.Sdk.Data;
using Xunit.Abstractions;

namespace Mathtone.Sdk.Data.Tests {

	public abstract class DbConnectionTestBase<CN> : IDBConnectionTestBase<CN> where CN : DbConnection {
		protected DbConnectionTestBase(ITestOutputHelper output) : base(output) {
		}

		[Fact]
		public async Task UsedAsync_NoResult() {
			var rslt = false;
			await Connect().UsedAsync(c => { rslt = true; });
			Assert.True(rslt);
		}

		[Fact]
		public async Task UsedAsync_Task_NoResult() {
			var rslt = false;
			await Connect().UsedAsync(c => Task.Run(() => { rslt = true; }));
			Assert.True(rslt);
		}

		[Fact]
		public async Task UsedAsync_Result() =>
			Assert.Equal("YES", await Connect().UsedAsync(c => "YES"));

		[Fact]
		public async Task UsedAsync_Task_Result() =>
			Assert.Equal("YES", await Connect().UsedAsync(c => Task.FromResult("YES")));
	}
}