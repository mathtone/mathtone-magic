namespace Mathtone.Sdk.Common.Services {
	public class SystemTimeService : ITimeService {
		public virtual DateTimeOffset Now => DateTimeOffset.Now;
		public virtual DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
		public DateTimeOffset In(TimeZoneInfo zone) => TimeZoneInfo.ConvertTime(Now, zone);

		DateTime ICurrentTime<DateTime, TimeZoneInfo>.Now => Now.DateTime;
		DateTime ICurrentTime<DateTime, TimeZoneInfo>.UtcNow => UtcNow.DateTime;
		DateTime ICurrentTime<DateTime, TimeZoneInfo>.In(TimeZoneInfo zone) => In(zone).DateTime;

		public DateTime MyTime(DateTime yourTimeUtc) => TimeZoneInfo.ConvertTime(yourTimeUtc, CurrentTimeZone);
		public DateTimeOffset MyTime(DateTimeOffset yourTime) => TimeZoneInfo.ConvertTime(yourTime, CurrentTimeZone);
		public virtual TimeZoneInfo CurrentTimeZone => TimeZoneInfo.Local;
	}

	//public interface ISystemTime : ILocalTime {
	//	new DateTimeOffset Now => DateTimeOffset.Now;
	//	new DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
	//}

	//public abstract class TimeService : ILocalTime, ITime {

	//	public virtual DateTimeOffset Now => DateTimeOffset.Now;
	//	public DateTimeOffset UtcNow => Now.UtcDateTime;

	//	DateTime ITime.Now => Now.DateTime;
	//	DateTime ITime.UtcNow => UtcNow.DateTime;
	//}

	//public struct TimeInZone {
	//	public DateTimeOffset Time { get; set; }
	//	public TimeZoneInfo Zone { get; set; }
	//}

	//public class FlexTimeService : TimeService {

	//	protected TimeInZone _startTime;
	//	readonly Stopwatch _stopwatch = new();
	//	public TimeZoneInfo TimeZone => _startTime.Zone;
	//	public TimeSpan Elapsed => _stopwatch.Elapsed;
	//	public override DateTimeOffset Now => _startTime.Time + _stopwatch.Elapsed;

	//	public FlexTimeService(bool timeFlows = true) :
	//		this(TimeZoneInfo.Local, timeFlows) { }

	//	public FlexTimeService(TimeZoneInfo timeZone, bool timeFlows = true) :
	//		this(DateTimeOffset.Now, timeZone, timeFlows) { }

	//	public FlexTimeService(DateTimeOffset startTime, bool timeFlows = true) :
	//		this(startTime, TimeZoneInfo.Local, timeFlows) { }

	//	public FlexTimeService(DateTimeOffset startTime, TimeZoneInfo timeZone, bool timeFlows = true) {
	//		Set(startTime, timeZone);
	//		if (timeFlows)
	//			Start();
	//	}

	//	public void Set(TimeZoneInfo currentZone) =>
	//		Set(_startTime.Time, currentZone);

	//	public void Set(DateTimeOffset currentTime) =>
	//		Set(currentTime, _startTime.Zone);

	//	public void Set(DateTimeOffset currentTime, TimeZoneInfo timeZone) =>
	//		Set(new TimeInZone {
	//			Time = TimeZoneInfo.ConvertTime(currentTime, timeZone),
	//			Zone = timeZone
	//		});

	//	public void Set(TimeInZone time) {
	//		_startTime = time;
	//	}

	//	public void Reset() => _stopwatch.Reset();

	//	public void Start() => _stopwatch.Start();

	//	public void Stop() => _stopwatch.Stop();

	//	public override TimeZoneInfo CurrentTimeZone => _startTime.Zone;
	//}

	//public class ClockService : TimeService {

	//	DateTimeOffset _baseTime;

	//	public ClockService() {
	//		//_baseTime = baseTime;
	//	}

	//	public override DateTimeOffset Now { get; }
	//}
}