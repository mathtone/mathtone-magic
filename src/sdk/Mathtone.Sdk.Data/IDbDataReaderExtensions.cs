using System.Data;

namespace Mathtone.Sdk.Data {
	public static class IDbDataReaderExtensions {

		public static IEnumerable<T> Consume<RDR, T>(this RDR reader, Func<RDR, T> selector) where RDR : IDataReader {
			while (reader.Read())
				yield return selector(reader);
		}
	}

	public static class IDataRecordExtensions {
		public static T Field<T>(this IDataRecord data, string name, Func<object?, T> selector) => selector(ToNull(data[name]));
		public static T Field<T>(this IDataRecord data, int index, Func<object?, T> selector) => selector(ToNull(data[index]));
		public static T? Field<T>(this IDataRecord data, string name) => (T?)Convert.ChangeType(ToNull(data[name]), typeof(T));
		public static T? Field<T>(this IDataRecord data, int index) => (T?)Convert.ChangeType(ToNull(data[index]), typeof(T));
		public static object? ToNull(object value) => value == Convert.DBNull ? null : value;
	}

}