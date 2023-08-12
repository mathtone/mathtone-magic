using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathtone.Sdk.Services {

	public abstract class ServiceBase {
		string? _serviceName;
		public virtual string ServiceName => _serviceName ??= GetType().Name;
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