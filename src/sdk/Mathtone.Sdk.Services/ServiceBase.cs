using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathtone.Sdk.Services {

	[SuppressMessage("Minor Code Smell", "S2094:Classes should not be empty", Justification = "<Pending>")]
	public abstract class ServiceBase {

	}

	public abstract class AppServiceBase : ServiceBase {
		protected ILogger Log;
		protected AppServiceBase(ILogger log) => Log = log;
	}

	public abstract class ConfiguredServiceBase<CFG> : AppServiceBase
		where CFG : class, new() {

		protected ConfiguredServiceBase(ILogger log, CFG config) : base(log) {
			Config = config;
		}

		protected CFG Config { get; }
	}
}