using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Mathtone.Sdk.Telemetry {

	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public class MetricAttribute : Attribute {

	}

	public static class TelemetryServiceCollectionExtensions {
		public static IServiceCollection AddTelemetry(this IServiceCollection services) =>
			services.AddSingleton(typeof(ITelemetry<>), typeof(TelemetryService<>));
	}

	public class TelemetryService<T> : TelemetryService, ITelemetry<T> {
		public TelemetryService(ILogger<T> log) :
			base(log) { }
	}

	public class TelemetryService : ITelemetry {

		protected ILogger Logger { get; }

		public TelemetryService(ILogger log) {
			Logger = log;
		}

		public IDisposable? BeginScope<TState>(TState state) where TState : notnull =>
			Logger.BeginScope(state);

		public void Collect<T>(IMetric<T> metric) {
			throw new NotImplementedException();
		}

		public bool IsEnabled(LogLevel logLevel) =>
			Logger.IsEnabled(logLevel);

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) =>
			Logger.Log(logLevel, eventId, state, exception, formatter);
	}

	public interface ITelemetry<out T> : ILogger<T> {
	}

	public interface ITelemetry : ILogger {
		void Collect<T>(IMetric<T> metric);
		void Collect<T>(CollectionType type, string name, string description, T value) => Collect<T>(new Metric<T>(type, name, description, value));
	}

	public interface IMetric {
		CollectionType CollectionType { get; }
		string Name { get;  }
		string Description { get; }
	}

	public interface IMetric<out T> : IMetric {
		T Value { get; }
	}

	public class Metric {

		public CollectionType CollectionType { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public Metric(CollectionType collectionType, string name, string description) {
			CollectionType = collectionType;
			Name = name;
			Description = description;
		}
	}

	public class Metric<T> : Metric, IMetric<T> {
		public Metric(CollectionType collectionType, string name, string description, T value) :
			base(collectionType, name, description) {
			Value = value;
		}

		public T Value { get; }
	}

	public enum CollectionType {
		Count, Sample, Summary
	}
}