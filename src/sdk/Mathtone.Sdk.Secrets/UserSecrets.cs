using Microsoft.Extensions.Configuration;

namespace Mathtone.Sdk.Secrets {
	public abstract class UserSecrets : ISecrets {
		
		protected abstract IConfigurationRoot Config { get; }
		public string GetSecret(string key) => Config[key]!;
	}
}
