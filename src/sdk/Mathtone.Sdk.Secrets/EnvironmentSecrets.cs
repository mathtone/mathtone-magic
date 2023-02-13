namespace Mathtone.Sdk.Secrets {
	public class EnvironmentSecrets : ISecrets {
		public EnvironmentSecrets() { }

		public string GetSecret(string key) => Environment.GetEnvironmentVariable(key)!;
	}
}
