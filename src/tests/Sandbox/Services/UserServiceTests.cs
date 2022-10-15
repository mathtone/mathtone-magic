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

	public class DictionaryRepository<ID, ITEM> : Dictionary<ID, ITEM>, IRepository<ID, ITEM>
		
		where ITEM : IIdentified<ID>
		where ID : notnull {

		public ID Create(ITEM item) {
			Add(item.Id, item);
			return item.Id;
		}

		public ITEM Read(ID id) => this[id];

		public void Update(ITEM item) => this[item.Id] = item;

		public void Delete(ID id) => Remove(id);
	}

	public interface IIdentified<out ID> {
		ID Id { get; }
	}

	public interface IRepository<ID, ITEM> {
		ID Create(ITEM item);
		ITEM Read(ID id);
		void Update(ITEM item);
		void Delete(ID id);
	}
}