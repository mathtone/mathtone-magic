using Mathtone.Sdk.Secrets;
using Microsoft.Extensions.Configuration;

namespace Mathtone.Secrets {
	public class MathtoneUserSecrets : UserSecrets {
		public MathtoneUserSecrets() {
			Config = new ConfigurationBuilder()
				.AddUserSecrets<MathtoneUserSecrets>()
				.Build();
		}
		protected override IConfigurationRoot Config { get; }
	}
}