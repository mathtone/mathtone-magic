using System.Data.Common;

namespace Mathtone.Sdk.Data {
	public static class DbConnectionExtensions {
		public static async Task UsedAsync<CN>(this CN connection, Action<CN> action, CancellationToken token = default) where CN : DbConnection {
			await connection.OpenAsync(token);
			try {
				action(connection);
			}
			finally {
				await connection.DisposeAsync();
			}
		}

		public static async Task UsedAsync<CN>(this CN connection, Func<CN, Task> asyncAction, CancellationToken token = default) where CN : DbConnection {
			await connection.OpenAsync(token);
			try {
				await asyncAction(connection);
			}
			finally {
				await connection.DisposeAsync();
			}
		}

		public static async Task<RSLT> UsedAsync<CN, RSLT>(this CN connection, Func<CN, RSLT> selector, CancellationToken token = default) where CN : DbConnection {
			await connection.OpenAsync(token);
			try {
				return selector(connection);
			}
			finally {
				await connection.DisposeAsync();
			}
		}

		public static async Task<RSLT> UsedAsync<CN, RSLT>(this CN connection, Func<CN, Task<RSLT>> asyncSelector, CancellationToken token = default) where CN : DbConnection {
			await connection.OpenAsync(token);
			try {
				return await asyncSelector(connection);
			}
			finally {
				await connection.DisposeAsync();
			}
		}
	}
}