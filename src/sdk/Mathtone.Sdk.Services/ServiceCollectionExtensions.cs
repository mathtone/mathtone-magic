using Microsoft.Extensions.DependencyInjection;

namespace Mathtone.Sdk.Services {
	public static class ServiceCollectionExtensions {
		
		public static IServiceCollection ActivateSingleton<SVC, IMPL>(this IServiceCollection services, params object[] parameters)=>
			services.AddActivator<SVC,IMPL>(ServiceLifetime.Singleton,parameters);

		public static IServiceCollection ActivateSingleton<SVC>(this IServiceCollection services, params object[] parameters) =>
			services.ActivateSingleton<SVC,SVC>(parameters);

		public static IServiceCollection ActivateTransient<SVC, IMPL>(this IServiceCollection services, params object[] parameters) =>
			services.AddActivator<SVC, IMPL>(ServiceLifetime.Transient, parameters);

		public static IServiceCollection ActivateTransient<SVC>(this IServiceCollection services, params object[] parameters) =>
			services.ActivateTransient<SVC,SVC>(parameters);

		public static IServiceCollection ActivateScoped<SVC, IMPL>(this IServiceCollection services, params object[] parameters) =>
			services.AddActivator<SVC, IMPL>(ServiceLifetime.Scoped, parameters);

		public static IServiceCollection ActivateScoped<SVC>(this IServiceCollection services, params object[] parameters) =>
			services.ActivateScoped<SVC, SVC>(parameters);

		public static IServiceCollection AddActivator<SVC>(this IServiceCollection services, ServiceLifetime lifetime, params object[] parameters) =>
			services.AddActivator<SVC, SVC>(lifetime, parameters);

		public static IServiceCollection AddActivator<SVC, IMPL>(this IServiceCollection services, ServiceLifetime lifetime, params object[] parameters) {
			services.Add(new ServiceDescriptor(typeof(SVC), svc => ActivatorUtilities.CreateInstance<IMPL>(svc, parameters)!, lifetime));
			return services;
		}
	}

	public static class IServiceProviderExtensions {
		public static SVC Activate<SVC>(this IServiceProvider services, params object[] parameters) =>
			ActivatorUtilities.CreateInstance<SVC>(services, parameters);
	}
}
