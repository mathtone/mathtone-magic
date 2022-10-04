using Mathtone.Sdk.Common.Extensions;
using Mathtone.Sdk.Common.Services.Time;
using System.Data;

namespace Mathtone.Sdk.Common.Tests {
	public class IEnumerableExtensionsTests {

		[Fact]
		public void ForEach() {

			var rslt = new List<int>();
			var items = new[] { 1, 2, 3, 4, 5 };
			items.ForEach(rslt.Add);
			Assert.Equal(items, rslt);
		}
	}

	public class SystemTimeServiceTests : TimeServiceTests<SystemTimeService> {

	}

	public class FlexTimeServiceTests : TimeServiceTests<FlexTimeService> {
		public FlexTimeServiceTests() {
		}

		[Fact]
		public async Task Reset() {

			var date = DateTimeOffset.Parse("1975-10-25 07:15:00 -7:00");
			var svc = new FlexTimeService(date);

			await Task.Delay(100);
			Assert.NotEqual(date, svc.Now);
			Assert.NotEqual(0, svc.Elapsed.TotalMilliseconds);


			svc.Stop();
			svc.Reset();
			Assert.Equal(date, svc.Now);
		}

		[Fact]
		public async Task Set() {
			var date = DateTimeOffset.Parse("1975-10-25 07:15:00 -7:00");
			var svc = new FlexTimeService(false);
			svc.Set(date);
			await Task.Delay(100);
			Assert.Equal(date, svc.Now);
		}

		[Fact]
		public void SetTimezone() {
			//var date = DateTimeOffset.Parse("1975-10-25 07:15:00 -7:00");
			var svc = new FlexTimeService(false);
			Assert.Equal(TimeZoneInfo.Local, svc.CurrentTimeZone);
			svc.Set(TimeZoneInfo.Utc);
			Assert.Equal(TimeZoneInfo.Utc, svc.CurrentTimeZone);
			//await Task.Delay(100);
			//Assert.Equal(date, svc.Now);
		}
	}

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