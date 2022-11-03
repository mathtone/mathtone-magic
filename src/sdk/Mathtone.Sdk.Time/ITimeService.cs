
﻿using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
namespace Mathtone.Sdk.Time {

	public interface ITime : ICurrentTime<DateTime> { }
	public interface ITimeOffset : ICurrentTime<DateTimeOffset> { }

	public interface ITimeService : ITime, ITimeOffset {
		TimeZoneInfo CurrentTimeZone { get; }
	}

	public interface ICurrentTime<out T> : ICurrentTime<T, TimeZoneInfo> { }
	public interface ICurrentTime<out T, in Z> {
		T Now { get; }
		T UtcNow { get; }
		T In(Z zone);
	}
	public struct TimeInZone {
		public DateTimeOffset Time { get; set; }
		public TimeZoneInfo Zone { get; set; }
	}

	public static class TimeInZoneExtensions {
		public static IServiceCollection AddTime<SVC>(this IServiceCollection services)
			where SVC : class, ITimeService => services
				.AddSingleton<ITimeService, SVC>()
				.AddSingleton<ITime>(svc => svc.GetRequiredService<ITimeService>())
				.AddSingleton<ITimeOffset>(svc => svc.GetRequiredService<ITimeService>());
	}
}