using Mathtone.Sdk.Testing.Xunit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace _Sandbox.Services {

	public class ServiceTestBase : XunitTestBase {
		public ServiceTestBase(ITestOutputHelper output) : base(output) {
		}
	}

	public class UserServiceTests : ServiceTestBase {
		public UserServiceTests(ITestOutputHelper output) : base(output) {
		}

		[Fact]
		public void TestUserService() {
			var svc = new ServiceCollection()
				.AddXunitLogging(Output)
				//.AddScoped<UserService>()
				.BuildServiceProvider();


		}
	}


}