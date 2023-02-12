namespace Mathtone.Sdk.Services {

	public abstract class ServiceBase {
		Type? _type;
		protected Type ServiceType => _type ??= GetType();
		protected string ServiceTypeName => ServiceType.Name;
	}

}