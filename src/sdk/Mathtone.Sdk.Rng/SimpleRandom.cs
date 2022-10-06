namespace Mathtone.Sdk.Rng {
	public class SimpleRandom : Random, IRandom {

		public SimpleRandom() : base() { }
		public SimpleRandom(int Seed) : base(Seed) {
		}

		public virtual byte[] GetBytes(int count) {
			var rtn = new byte[count];
			NextBytes(rtn);
			return rtn;
		}
	}
}