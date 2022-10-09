namespace Mathtone.Sdk.Time {
	public interface ITimeService : ICurrentTime<DateTime>, ICurrentTime<DateTimeOffset> {
		TimeZoneInfo CurrentTimeZone { get; }
	}

	public interface ICurrentTime<T> : ICurrentTime<T, TimeZoneInfo> { }
	public interface ICurrentTime<T, in Z> {
		T Now { get; }
		T UtcNow { get; }
		T In(Z zone);
		//T MyTime(T yourTime);
	}
	public struct TimeInZone {
		public DateTimeOffset Time { get; set; }
		public TimeZoneInfo Zone { get; set; }
	}

}