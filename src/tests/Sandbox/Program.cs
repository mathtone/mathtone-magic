using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Xml.Linq;
using _Sandbox.Logging.Console;
using System.Data.SqlClient;
//using Mathtone.Sdk.Data;

namespace Sandbox {

	public class Program {

		[Fact]
		public void Test1() {

		}


		static void Main(string[] args) {

			var x = new[] { 1, 2, 3, 4, 5 };
			//x.ForeachAwaitAsync

			using IHost host = Host
				.CreateDefaultBuilder(args)
				.ConfigureLogging(builder => {
					//builder.ClearProviders();
					//builder.AddSimpleConsole(options => {
					//	options.IncludeScopes = true;
					//	options.SingleLine = true;
					//	options.TimestampFormat = "hh:mm:ss.fff ";
					//});
					//builder.AddJsonConsole();
					//builder.AddSystemdConsole(options => {
					//	options.IncludeScopes = true;
					//	options.TimestampFormat = "hh:mm:ss.fff ";
					//});
					//builder.AddConsoleLogger();
					builder.AddColorConsoleLogger();
				})
				.Build();

			var logger = host.Services.GetRequiredService<ILogger<Microsoft.VisualStudio.TestPlatform.TestHost.Program>>();

			logger.LogDebug(1, "Does this line get hit?");    // Not logged
			logger.LogInformation(3, "Nothing to see here."); // Logs in ConsoleColor.DarkGreen
			logger.LogWarning(5, "Warning... that was odd."); // Logs in ConsoleColor.DarkCyan
			logger.LogError(7, "Oops, there was an error.");  // Logs in ConsoleColor.DarkRed
			logger.LogTrace(5!, "== 120.");                   // Not logged

			//await host.RunAsync();
		}
	}

	
}