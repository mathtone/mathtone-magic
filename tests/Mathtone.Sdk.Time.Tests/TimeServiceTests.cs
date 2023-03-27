using Mathtone.Sdk.Time;
using Microsoft.Extensions.DependencyInjection;

namespace Mathtone.Sdk.Time.Tests {
	public class TimeServiceExtensionsTests {
		[Fact]
		public void AddTime_ShouldAddServices() {
			var svc = new ServiceCollection()
				.AddTime<SystemTimeService>()
				.BuildServiceProvider();
			Assert.NotNull(svc.GetRequiredService<ITimeService>());
			Assert.NotNull(svc.GetRequiredService<ITime>());
			Assert.NotNull(svc.GetRequiredService<ITimeOffset>());
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


		//[Fact]
		//public void AddTime_ShouldAddTimeService() {
		//	// Arrange
		//	//var services = new ServiceCollection().Cr.BuildServiceProvider();

		//	//// Act
		//	//services.AddTime<MockTimeService>();

		//	//// Assert
		//	//Assert.NotNull(services ITimeService));
		//	//Assert.Contains(services, s => s.ServiceType == typeof(ITime));
		//	//Assert.Contains(services, s => s.ServiceType == typeof(ITimeOffset));
		//}


		//[Fact]
		//public void AddTime_ShouldAddTimeService2() {
		//	// Arrange
		//	var services = new ServiceCollection();

		//	// Act
		//	services.AddTime();

		//	// Assert
		//	Assert.Contains(services, s => s.ServiceType == typeof(ITimeService));
		//	Assert.Contains(services, s => s.ServiceType == typeof(ITime));
		//	Assert.Contains(services, s => s.ServiceType == typeof(ITimeOffset));
		//}
	}

	//public class MockTimeService : ITimeService {
	//	public DateTime Now => DateTime.Now;
	//	public DateTime UtcNow => DateTime.UtcNow;
	//	public TimeZoneInfo TimeZone => TimeZoneInfo.Local;

	//	public TimeZoneInfo CurrentTimeZone { get; } = TimeZoneInfo.Local;
	//	DateTimeOffset ICurrentTime<DateTimeOffset, TimeZoneInfo>.Now { get; }
	//	DateTimeOffset ICurrentTime<DateTimeOffset, TimeZoneInfo>.UtcNow { get; }

	//	public DateTime In(TimeZoneInfo zone) {
	//		throw new NotImplementedException();
	//	}

	//	DateTimeOffset ICurrentTime<DateTimeOffset, TimeZoneInfo>.In(TimeZoneInfo zone) {
	//		throw new NotImplementedException();
	//	}
	//}
}

