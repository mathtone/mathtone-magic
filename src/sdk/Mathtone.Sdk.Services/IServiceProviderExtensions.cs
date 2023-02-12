using Microsoft.Extensions.DependencyInjection;

namespace Mathtone.Sdk.Services {
	public static class IServiceProviderExtensions {
		public static SVC Activate<SVC>(this IServiceProvider services, params object[] parameters) =>
			ActivatorUtilities.CreateInstance<SVC>(services, parameters);

		public static SVC Activate<SVC, IMPL>(this IServiceProvider services, params object[] parameters) where IMPL : SVC =>
			services.Activate<IMPL>(parameters);

		public static SVC Activated<SVC>(this IServiceProvider services, params object[] parameters) =>
			services.GetRequiredService<IActivator<SVC>>().Activate(parameters);

		public static SVC Activated<SVC, ARG>(this IServiceProvider services, ARG arg) =>
			services.GetRequiredService<IActivator<SVC, ARG>>().Activate(arg);
	}
}