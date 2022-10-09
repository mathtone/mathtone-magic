using Mathtone.Sdk.Testing.Xunit;
using Xunit.Abstractions;

namespace Mathtone.Sdk.Rng.Tests {
	public class IRandomExtensionsTests : XunitTestBase {

		readonly IRandom _rng = new SimpleRandom(100);

		public IRandomExtensionsTests(ITestOutputHelper output) : base(output) {
		}

		[Fact]
		public void Random_FromList() {
			var items = Enumerable.Range(0, 10).ToList();
			var r = _rng.Random(items);
			Assert.Equal(9, r);
		}


		[Fact]
		public void Random_FromEnumerable() {
			var items = Enumerable.Range(0, 10);
			var r = _rng.Random(items);
			Assert.Equal(9, r);
		}

		[Fact]
		public void Randomize_List() {
			var items = Enumerable.Range(0, 5).ToList();
			var r = _rng.Randomize(items).ToArray();
			Assert.Equal(new[] { 3, 1, 2, 0, 4 }, r);
		}

		[Fact]
		public void Randomize_IEnumerable() {
			var items = Enumerable.Range(0, 5);
			var r = _rng.Randomize(items).ToArray();
			Assert.Equal(new[] { 3, 1, 2, 0, 4 }, r);
		}

		//[Theory]
		//[InlineData(AlphaNumInclude.All, 20, "-lX%A_1Tzl-NBK=J%+Cc")]
		//[InlineData(AlphaNumInclude.Lower, 20, "zerxjyspjdzojmzmxzja")]
		//[InlineData(AlphaNumInclude.Upper, 20, "ZERXJYSPJDZOJMZMXZJA")]
		//[InlineData(AlphaNumInclude.Number, 20, "02704087420645059041")]
		//[InlineData(AlphaNumInclude.Symbol, 20, "+@_=%+_*%@+&%^+^=+%!")]
		//[InlineData(AlphaNumInclude.AlphaNum, 20, "9jP4v7SLvj8HxE9D40xc")]
		//[InlineData(AlphaNumInclude.AlphaNumUpper, 20, "9FY7M9ZVMF9TNR0R70NB")]
		//[InlineData(AlphaNumInclude.AlphaNumLower, 20, "9fy7m9zvmf9tnr0r70nb")]
		//[InlineData(AlphaNumInclude.All, 200, "-lX%A_1Tzl-NBK=J%+CchC%-L!^VmTh7LKPh#NI3uKyRU@Ab^Gc7v@qRtZDLceG3dmc$*$TBMV5SdwyVJ$33RxKWu=0mj_ZpeSOI0Jgc9WtWhTN-A5bly1k2XCS-dHrQ_OOH4#9a8Z^&e!OXG#8B*O$zH97H#j=BFZZOegpJ_bAU8pX90*MnW#UDXp+*gAkX2NLAk+Sz")]
		//public void Random_GetString(AlphaNumInclude include,int count, string expected) {
		//	var actual = _rng.GetString(count, include);
		//	Output.WriteLine(actual);
		//	Assert.Equal(count, actual.Length);
		//	Assert.Equal(expected, actual);
		//}

		[Theory]
		[InlineData(64, "GlYo8DawrVb2LHLjJnkddgn6J6Y1WbbOIhK6wh8fZJN6i6zCdRTF0AVqF3wTdK1hyvFyMR8ZusGfWc4j9us31g==")]
		public void Random_Base64String(int bytes, string expected) {
			var actual = _rng.Base64String(bytes);
			Output.WriteLine(actual);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(64, "1A5628F036B0AD56F62C72E326791D7609FA27A63559B6CE2212BAC21F1F64937A8BACC27514C5D0056A177C1374AD61CAF172311F19BAC19F59CE23F6EB37D6")]
		public void Random_HexString(int bytes, string expected) {
			var actual = _rng.HexString(bytes);
			Output.WriteLine(actual);
			Assert.Equal(expected, actual);
		}

		//[Fact]
		//public void Random_GetStringAlphaNum() {
		//	var actual = _rng.GetString(20, AlphaNumInclude.Alpha);
		//	Output.WriteLine(actual);
		//	Assert.Equal("YiIUsXLFshYCtzYzUZtb", actual);
		//	;
		//}
	}
}