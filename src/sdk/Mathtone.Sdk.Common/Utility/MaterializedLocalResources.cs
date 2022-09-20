using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace Mathtone.Sdk.Common.Utility {
	public class MaterializedLocalResources {

		public MaterializedLocalResources() {

			var flags = BindingFlags.Public | BindingFlags.Instance;
			var type = GetType();
			var nameRoot = type.Assembly.GetName().Name;
			var attr = type.GetCustomAttribute<ResourcePathAttribute>(true);
			var searchPath = attr == null ? $"{type.FullName}." : $"{nameRoot}.{attr.ResourcePath}.";
			var l = searchPath!.Length;
			var resources = type.Assembly
				.GetManifestResourceNames()
				.Where(r => r.StartsWith(searchPath))
				.ToDictionary(
					k => Path.GetFileNameWithoutExtension(k).Remove(0, l),
					v => v
				);

			foreach (var mem in type.GetMembers(flags)) {

				if (mem is PropertyInfo prop) {
					HandleProperty(prop, resources);
				}
				else if (mem is FieldInfo field) {
					HandleField(field, resources);
				}
			}
		}

		void HandleProperty(PropertyInfo property, IDictionary<string, string> resources) {
			var attr = property.GetCustomAttribute<ResourceAttribute>();
			var name = attr?.ResourceName ?? property.Name;
			if (resources.ContainsKey(name)) {
				var n = resources[name];
				var v = property.DeclaringType!.Assembly.GetResource(n)!;
				property.SetValue(this, v);
			}
		}

		void HandleField(FieldInfo field, IDictionary<string, string> resources) {
			var attr = field.GetCustomAttribute<ResourceAttribute>();
			var name = attr?.ResourceName ?? field.Name;
			if (resources.ContainsKey(name)) {
				var n = resources[name];
				var v = field.DeclaringType!.Assembly.GetResource(n)!;
				field.SetValue(this, v);
			}
		}
	}

	public static class AssemblyExtensions {
		public static string GetResource(this Assembly assembly, string resourceName) {
			using var resourceStream = assembly.GetManifestResourceStream(resourceName)!;
			using var r = new StreamReader(resourceStream);
			return r.ReadToEnd();
		}
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class ResourceAttribute : Attribute {

		public string? ResourceName { get; }

		public ResourceAttribute(string? name = default) {
			ResourceName = name;
		}
	}

	[AttributeUsage(AttributeTargets.Class)]
	public class ResourcePathAttribute : Attribute {

		public string? ResourcePath { get; }

		public ResourcePathAttribute(string? resourcePath = default) {
			ResourcePath = resourcePath;
		}
	}
}