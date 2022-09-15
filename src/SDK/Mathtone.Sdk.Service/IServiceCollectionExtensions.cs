using Microsoft.Extensions.DependencyInjection;

namespace Mathtone.Sdk.Service {
	public static class IServiceCollectionExtensions {
		public static IServiceCollection AddActivator<SVC, IMPL>(this IServiceCollection services, ServiceLifetime lifetime, params object[] args) {
			var descriptor = new ServiceDescriptor(typeof(SVC), svc => ActivatorUtilities.CreateInstance(svc, typeof(IMPL), args), lifetime);
			services.Add(descriptor);
			return services;
		}
	}
}