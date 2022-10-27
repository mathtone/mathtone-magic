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
}