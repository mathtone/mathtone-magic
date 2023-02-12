using Microsoft.Extensions.Logging;

namespace Mathtone.Sdk.Services {
	public abstract class LoggedServiceBase {

		protected LoggedServiceBase(ILogger<LoggedServiceBase> logger) {
			Log = logger;
		}

		protected readonly ILogger<LoggedServiceBase> Log;
	}
}