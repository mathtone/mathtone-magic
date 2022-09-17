namespace Mathtone.Sdk.Common {
	public interface IIdentified<out ID> {
		ID Id { get; }
	}

	public class Identified<ID> : IIdentified<ID> {

		public ID Id { get; set; }

		public Identified(ID id) =>
			Id = id;
	}
}