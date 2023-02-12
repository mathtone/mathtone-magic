namespace Mathtone.Sdk.Services {
	public interface IActivator<out SVC> {
		SVC Activate(params object[] args);
	}

	public interface IActivator<out SVC,in ARG> {
		SVC Activate(ARG arg);
	}
}