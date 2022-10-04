using Mathtone.Sdk.Common.Services.Time;

namespace Mathtone.Sdk.Services.Tests.Time {
	public abstract class TimeServiceTests<T> where T : ITimeService, new() {

		[Fact]
		public void Now_Dto() {
			var svc = new T() as ICurrentTime<DateTimeOffset>;
			var now = svc.Now;
			Assert.True((DateTimeOffset.Now - now).TotalMilliseconds < 10);
		}

		[Fact]
		public void UtcNow_Dto() {
			var svc = new T() as ICurrentTime<DateTimeOffset>;
			var now = svc.UtcNow;
			Assert.True((DateTimeOffset.UtcNow - now).TotalMilliseconds < 10);
		}

		[Fact]
		public void Now_Dt() {
			var svc = new T() as ICurrentTime<DateTime>;
			var now = svc.Now;
			Assert.True((DateTime.Now - now).TotalMilliseconds < 10);
		}

		[Fact]
		public void UtcNow_Dt() {
			var svc = new T() as ICurrentTime<DateTime>;
			var now = svc.UtcNow;
			Assert.True((DateTime.UtcNow - now).TotalMilliseconds < 10);
		}

		[Fact]
		public void GetTimezone() {
			//var date = DateTimeOffset.Parse("1975-10-25 07:15:00 -7:00");
			var svc = new T();
			Assert.Equal(TimeZoneInfo.Local, svc.CurrentTimeZone);
		}

		[Fact]
		public void GetTimeInZone_Dto() {
			//var date = DateTimeOffset.Parse("1975-10-25 07:15:00 -7:00");
			var svc = new T() as ICurrentTime<DateTimeOffset>;
			var diff = svc.In(TimeZoneInfo.Utc) - svc.UtcNow;
			Assert.True(diff.TotalMilliseconds < 100);
		}

		[Fact]
		public void GetTimeInZone_Dt() {
			//var date = DateTimeOffset.Parse("1975-10-25 07:15:00 -7:00");
			var svc = new T() as ICurrentTime<DateTime>;
			var diff = svc.In(TimeZoneInfo.Utc) - svc.UtcNow;
			Assert.True(diff.TotalMilliseconds < 100);
		}

		//[Fact]
		//public void MyTime() {
		//	//var date = DateTimeOffset.Parse("1975-10-25 07:15:00 -7:00");
		//	var svc = new T() as ICurrentTime<DateTimeOffset>;
		//	var now = svc.Now;
		//	var utc = svc.Now.ToUniversalTime();
		//	Assert.Equal(now, svc.MyTime(utc));
		//	//Assert.True(diff.TotalMilliseconds < 100);
		//}
	}
}