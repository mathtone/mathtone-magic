using Mathtone.Sdk.Common.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Text;
using Xunit.Abstractions;

namespace Mathtone.Sdk.Logging.Tests {

	public class UnitTest1 {

		readonly XunitOutputAdapter _output;

		public UnitTest1(ITestOutputHelper output) {
			_output = new(output);
		}

		[Fact]
		public async Task Test1() {
			var logger = XunitLogger.Create<UnitTest1>(_output);
			logger.LogInformation("TEST");
		}
	}

	
}