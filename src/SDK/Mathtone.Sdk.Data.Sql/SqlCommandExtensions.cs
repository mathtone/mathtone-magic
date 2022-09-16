
using System.Data;
using System.Data.SqlClient;

namespace Mathtone.Sdk.Data.Sql {
	public static class SqlCommandExtensions {
		public static SqlCommand WithParameter<T>(this SqlCommand command, string name, T value, ParameterDirection direction, SqlDbType type, int size = default) {
			var p = command.CreateParameter();
			p.ParameterName = name;
			p.Value = value;
			p.Direction = direction;
			p.Size = size;
			p.SqlDbType = type;
			return command.WithParameter(p);
		}

		public static SqlCommand WithInput<T>(this SqlCommand command, string name, T value, SqlDbType type, int size = default) =>
			command.WithParameter(name, value, ParameterDirection.Input, type, size);

	}
}