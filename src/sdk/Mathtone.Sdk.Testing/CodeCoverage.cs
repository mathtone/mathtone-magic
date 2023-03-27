using Mathtone.Sdk.Testing.Properties;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Mathtone.Sdk.Testing {
	public class CodeCoverage {

		static readonly BindingFlags CoverageFlags =
			BindingFlags.DeclaredOnly |
			BindingFlags.NonPublic |
			BindingFlags.Public |
			BindingFlags.Instance |
			BindingFlags.Static;

		public static void CoverInstance(object? instance, ILogger logger) {
			if (instance == null) {
				logger.LogWarning(Resources.CouldNotConstruct, nameof(instance));
				return;
			}
			var type = instance.GetType();
			var errors = 0;

			foreach (var property in type.GetProperties(CoverageFlags)) {
				var defaultValue = DefaultValue(property.PropertyType);
				if (property.CanRead) {
					try {
						defaultValue = property.GetValue(instance);
					}
					catch {
						errors++;
					}
				}
				if (property.CanWrite) {
					try {
						property.SetValue(instance, defaultValue);
					}
					catch {
						logger.LogWarning(Resources.CouldNotCoverType, instance.GetType());
						errors++;
					}
				}
			}
		}

		public static void CoverType<T>(ILogger logger) =>
			CoverType(typeof(T), logger);

		public static void CoverType(Type type, ILogger log) =>
			CoverInstance(DefaultInstance(type), log);

		public static void CoverProperties<T>(ILogger log) =>
			CoverType(typeof(T), log);

		public static void CoverAssembly(Assembly assembly, ILogger log) {
			foreach (var t in assembly.GetTypes().Where(a => !a.IsInterface)) {
				try {
					CoverType(t, log);
				}
				catch {
					Console.WriteLine(string.Format(Resources.CouldNotConstruct, t.Name));
				}
			}
		}

		static object? DefaultInstance(Type type) {
			try {
				return Activator.CreateInstance(type);
			}
			catch (MissingMethodException) {
				var constructors = type.GetConstructors();
				foreach (var constructor in constructors) {
					var parameters = constructor.GetParameters().Select(a => DefaultValue(a.ParameterType)).ToArray();
					try {
						return Activator.CreateInstance(type, parameters);
					}
					catch {

					}
				}
				return null;
			}
		}

		static object? DefaultValue(Type type) =>
			type.IsValueType ? Activator.CreateInstance(type) : null;
	}
}
