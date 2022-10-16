namespace Mathtone.Sdk.Common {
	public interface IIdentified<out ID> {
		ID Id { get; }
	}
}
