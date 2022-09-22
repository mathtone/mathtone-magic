using System.Data;

namespace Mathtone.Sdk.Data {
	public static class IDataReaderExtensions {

		public static IEnumerable<T> Consume<RDR, T>(this RDR reader, Func<RDR, T> selector) where RDR : IDataReader {
			while (reader.Read())
				yield return selector(reader);
		}

		//public static IEnumerable<T> Consume<RDR, T>(this RDR reader, Func<RDR, Task<T>> selector) where RDR : IDataReader {
		//	while (reader.Read())
		//		yield return selector(reader).Result;
		//}
	}
}