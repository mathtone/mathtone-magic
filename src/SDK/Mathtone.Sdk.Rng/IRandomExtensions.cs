namespace Mathtone.Sdk.Rng {
	public static class IRandomExtensions {
		public static Guid NewGuid(this IRandom rng) => new(rng.GetBytes(16));
	}
}