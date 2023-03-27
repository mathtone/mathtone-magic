using Mathtone.Sdk.Data.Tests.Support;

namespace Mathtone.Sdk.Data.Tests {
	public class DBCommandExtensionsTest {

		[Fact]
		public async Task ExecuteScalar() {

			Assert.Equal("TEST", await new TestDbCommand().ExecuteScalarAsync(r => r));

		}
	}
}