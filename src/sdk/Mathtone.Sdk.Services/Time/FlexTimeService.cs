using System.Diagnostics;

namespace Mathtone.Sdk.Common.Services.Time {
	public class FlexTimeService : SystemTimeService {

		protected TimeInZone _startTime;
		readonly Stopwatch _stopwatch = new();
		public override TimeZoneInfo CurrentTimeZone => _startTime.Zone;
		public TimeSpan Elapsed => _stopwatch.Elapsed;
		public override DateTimeOffset Now => _startTime.Time + _stopwatch.Elapsed;
		public override DateTimeOffset UtcNow => Now.ToUniversalTime();

		public FlexTimeService() :
			this(true) { }

		public FlexTimeService(bool timeFlows) :
			this(TimeZoneInfo.Local, timeFlows) { }

		public FlexTimeService(TimeZoneInfo timeZone, bool timeFlows = true) :
			this(DateTimeOffset.Now, timeZone, timeFlows) { }

		public FlexTimeService(DateTimeOffset startTime, bool timeFlows = true) :
			this(startTime, TimeZoneInfo.Local, timeFlows) { }

		public FlexTimeService(DateTimeOffset startTime, TimeZoneInfo timeZone, bool timeFlows = true) {
			Set(startTime, timeZone);
			if (timeFlows)
				Start();
		}

		public void Set(TimeZoneInfo currentZone) =>
			Set(_startTime.Time, currentZone);

		public void Set(DateTimeOffset currentTime) =>
			Set(currentTime, _startTime.Zone);

		public void Set(DateTimeOffset currentTime, TimeZoneInfo timeZone) =>
			Set(new TimeInZone {
				Time = TimeZoneInfo.ConvertTime(currentTime, timeZone),
				Zone = timeZone
			});

		public void Set(TimeInZone time) {
			_startTime = time;
		}

		public void Reset() => _stopwatch.Reset();

		public void Start() => _stopwatch.Start();

		public void Stop() => _stopwatch.Stop();
	}
}