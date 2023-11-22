namespace Mathtone.Sdk.Telemetry.Abstractions {
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public class MetricAttribute(string name, string description, CollectionType collectionType) : Attribute {

		public string Name => name;
		public string Description => description;
		public CollectionType CollectionType => collectionType;
	}
}