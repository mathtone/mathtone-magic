using Microsoft.Extensions.DependencyInjection;
using static Microsoft.Extensions.DependencyInjection.ServiceLifetime;
namespace Mathtone.Sdk.Services {
	public static class ServiceCollectionExtensions {
		
		public static IServiceCollection ActivateSingleton<SVC, IMPL>(this IServiceCollection services, params object[] parameters)=>
			services.AddActivator<SVC,IMPL>(Singleton,parameters);

		public static IServiceCollection ActivateSingleton<SVC>(this IServiceCollection services, params object[] parameters) =>
			services.ActivateSingleton<SVC,SVC>(parameters);

		public static IServiceCollection ActivateTransient<SVC, IMPL>(this IServiceCollection services, params object[] parameters) =>
			services.AddActivator<SVC, IMPL>(Transient, parameters);

		public static IServiceCollection ActivateTransient<SVC>(this IServiceCollection services, params object[] parameters) =>
			services.ActivateTransient<SVC,SVC>(parameters);

		public static IServiceCollection ActivateScoped<SVC, IMPL>(this IServiceCollection services, params object[] parameters) =>
			services.AddActivator<SVC, IMPL>(Scoped, parameters);

		public static IServiceCollection ActivateScoped<SVC>(this IServiceCollection services, params object[] parameters) =>
			services.ActivateScoped<SVC, SVC>(parameters);

		public static IServiceCollection AddActivator<SVC>(this IServiceCollection services, ServiceLifetime lifetime, params object[] parameters) =>
			services.AddActivator<SVC, SVC>(lifetime, parameters);

		public static IServiceCollection AddActivator<SVC, IMPL>(this IServiceCollection services, ServiceLifetime lifetime, params object[] parameters) {
			services.Add(new ServiceDescriptor(typeof(SVC), svc => ActivatorUtilities.CreateInstance<IMPL>(svc, parameters)!, lifetime));
			return services;
		}
	}
}
