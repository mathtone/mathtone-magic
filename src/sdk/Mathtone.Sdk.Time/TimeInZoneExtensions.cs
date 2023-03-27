
using Microsoft.Extensions.DependencyInjection;
namespace Mathtone.Sdk.Time {
	public static class TimeServiceExtensions {
		public static IServiceCollection AddTime<SVC>(this IServiceCollection services)
			where SVC : class, ITimeService => services
				.AddSingleton<ITimeService, SVC>()
				.AddSingleton<ITime>(svc => svc.GetRequiredService<ITimeService>())
				.AddSingleton<ITimeOffset>(svc => svc.GetRequiredService<ITimeService>());

		//add xunit test please

	}


}