using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathtone.Sdk.Data.Services {
	public interface IDbConnectionProvider<out CN> where CN : IDbConnection {
		CN Create();
	}

	public interface IDbConnectionProvider<in ID, out CN> : IDbConnectionProvider<CN> where CN : IDbConnection {
		CN Create(ID id);
	}

	public static class IDbConnectionProviderExtensions {

		public static CN Open<CN>(this IDbConnectionProvider<CN> provider) where CN : IDbConnection {
			var cn = provider.Create();
			cn.Open();
			return cn;
		}

		public static async Task<CN> OpenAsync<CN>(this IDbConnectionProvider<CN> provider) where CN : IDbConnection {
			var cn = provider.Create();
			if (cn is DbConnection dbc) {
				await dbc.OpenAsync();
			}
			else {
				cn.Open();
			}
			return cn;
		}
	}
}