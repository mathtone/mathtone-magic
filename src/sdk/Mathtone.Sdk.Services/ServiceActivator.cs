using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathtone.Sdk.Services {
	public class ServiceActivator<SVC> : ServiceActivator<SVC, SVC> {
		public ServiceActivator(IServiceProvider services) :
			base(services) { }
	}

	public class ServiceActivator<SVC, IMPL> : IActivator<SVC> where IMPL : SVC {

		public ServiceActivator(IServiceProvider services) =>
			_services = services;

		private readonly IServiceProvider _services;

		public SVC Activate(params object[] args) => _services.Activate<IMPL>(args);
	}

	public class ServiceActivator<SVC, IMPL, ARG> : IActivator<SVC, ARG> where IMPL : SVC {

		public ServiceActivator(IServiceProvider services) =>
			_services = services;

		private readonly IServiceProvider _services;

		public SVC Activate(ARG arg) => _services.Activate<SVC, IMPL>(arg!);
	}
}
