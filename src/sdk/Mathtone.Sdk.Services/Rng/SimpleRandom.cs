namespace Mathtone.Sdk.Services.Rng {
	public class SimpleRandom : Random, IRandom {

		public byte[] GetBytes(int count) {
			var rtn = new byte[count];
			NextBytes(rtn);
			return rtn;
		}
	}
}