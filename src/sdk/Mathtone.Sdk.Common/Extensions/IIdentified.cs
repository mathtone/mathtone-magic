namespace Mathtone.Sdk.Common {
	public interface IIdentified<out ID> {
		ID Id { get; }
	}

	public class Identified<ID> : IIdentified<ID> {
		public Identified(ID id) {
			Id = id;
		}
		public ID Id { get; }
	}

	public class Identified<ID, VALUE> : Identified<ID> {
		public Identified(ID id, VALUE value) : base(id) {
			Value = value;
		}
		public VALUE Value { get; set; }
	}
}
