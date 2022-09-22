using System.Data.Common;

namespace Mathtone.Sdk.Data {
	public static class DbDataReaderExtensions {
		public static async IAsyncEnumerable<T> ConsumeAsync<RDR, T>(this RDR reader, Func<RDR, T> selector)
			where RDR : DbDataReader {
			while (await reader.ReadAsync())
				yield return selector(reader);
		}

		//public static async IAsyncEnumerable<T> ConsumeAsync<RDR, T>(this RDR reader, Func<RDR, Task<T>> selector)
		//	where RDR : DbDataReader {
		//	while (await reader.ReadAsync())
		//		yield return await selector(reader);
		//}


		public static async IAsyncEnumerable<T> ConsumeAsync<RDR, T>(this Task<RDR> reader, Func<RDR, T> selector)
			where RDR : DbDataReader {
			await foreach (var r in (await reader).ConsumeAsync(selector)) {
				yield return r;
			}
		}

		//public static async IAsyncEnumerable<T> ConsumeAsync<RDR, T>(this Task<RDR> reader, Func<RDR, Task<T>> selector)
		//	where RDR : DbDataReader {
		//	await foreach (var r in (await reader).ConsumeAsync(async r => await selector(r))) {
		//		yield return r;
		//	}
		//}
	}
}