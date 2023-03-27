namespace Mathtone.Sdk.Time.Tests {
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
}