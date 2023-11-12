using Mathtone.Sdk.Services;
using Mathtone.Sdk.Testing.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.Common.Exceptions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Xunit.Abstractions;

namespace Mathtone.Sdk.Telemetry.Tests {

	public class UnitTest1 : XunitServiceTestBase {
		public UnitTest1(ITestOutputHelper output) :
			base(output) { }

		protected override IServiceCollection OnConfigureServices(IServiceCollection services) => base
			.OnConfigureServices(services)
			.AddSingleton<IMeter, MetricsTestService>();

		[Fact]
		public void Test1() {
			var t = Activate<Telemetry<int>>();
			t.TestMessage();
		}
	}

	internal static partial class Logging {

		[LoggerMessage(0, LogLevel.Information, "Test message")]
		public static partial void TestMessage(this ITelemetry log);
	}

	public class Telemetry : ITelemetry {

		readonly ILogger _log;
		private readonly IMeter _metrics;

		public Telemetry(ILogger logger, IMeter metrics) {
			_log = logger;
			_metrics = metrics;
		}

		public virtual IDisposable? BeginScope<TState>(TState state) where TState : notnull =>
			_log.BeginScope(state);

		public void Collect<T>(T metric) where T : IMetric =>
			_metrics.Collect(metric);

		public virtual bool IsEnabled(LogLevel logLevel) =>
			_log.Equals(logLevel);

		public virtual void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) =>
			_log.Log(logLevel, eventId, state, exception, formatter);
	}

	public class Telemetry<T> : Telemetry {
		public Telemetry(ILogger<T> logger, IMeter metrics) :
			base(logger, metrics) { }
	}

	public class MetricsTestService : IMeter {
		public void Collect<T>(T metric) where T : IMetric {
			throw new NotImplementedException();
		}
	}

	public interface ITelemetry : ILogger, IMeter {

	}

	public interface IMeter {
		void Collect<T>(T metric) where T : IMetric;
	}

	public interface IMetric {

	}

	public interface IMetric<T> : IMetric {
		T Value { get; }
	}

	public readonly struct Metric<T> : IMetric<T> {
		public Metric(CollectionType type, string name, T value) =>
			(Type, Name, Value) = (type, name, value);

		public CollectionType Type { get; }
		public string Name { get; }
		public T Value { get; }
	}

	public class MetricAttribute {

	}

	public enum CollectionType {
		Counter,
		Gauge,
		Histogram,
		Summary
	}
}