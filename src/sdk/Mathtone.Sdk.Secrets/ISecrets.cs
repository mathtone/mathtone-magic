using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathtone.Sdk.Secrets {

	public interface ISecrets {
		public string GetSecret(string key);
		string this[string key] => GetSecret(key);
	}
}
