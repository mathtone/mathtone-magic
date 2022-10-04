namespace Mathtone.Sdk.Common.Services.Time {
	public class SystemTimeService : ServiceBase, ITimeService {
		public virtual DateTimeOffset Now => DateTimeOffset.Now;
		public virtual DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
		public DateTimeOffset In(TimeZoneInfo zone) => TimeZoneInfo.ConvertTime(Now, zone);

		DateTime ICurrentTime<DateTime, TimeZoneInfo>.Now => Now.DateTime;
		DateTime ICurrentTime<DateTime, TimeZoneInfo>.UtcNow => UtcNow.DateTime;
		DateTime ICurrentTime<DateTime, TimeZoneInfo>.In(TimeZoneInfo zone) => In(zone).DateTime;

		public virtual TimeZoneInfo CurrentTimeZone => TimeZoneInfo.Local;
	}
}