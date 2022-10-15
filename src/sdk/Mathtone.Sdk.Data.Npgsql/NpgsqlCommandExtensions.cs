using Mathtone.Sdk.Data;
using Mathtone.Sdk.Data.Npgsql;
using Npgsql;
using NpgsqlTypes;
using System.Data;


namespace Mathtone.Sdk.Data.Npgsql {
	public static class NpgsqlCommandExtensions {

		public static NpgsqlCommand WithParameter<T>(this NpgsqlCommand command, string name, T value, ParameterDirection direction, NpgsqlDbType type, int size = default) {
			var p = command.CreateParameter();
			p.ParameterName = name;
			p.Value = value;
			p.Direction = direction;
			p.Size = size;
			p.NpgsqlDbType = type;			
			return command.WithParameter(p);
		}

		public static NpgsqlCommand WithInput<T>(this NpgsqlCommand command, string name, T value, NpgsqlDbType type, int size = default) =>
			command.WithParameter(name, value, ParameterDirection.Input, type, size);

		//public static SqlCommand WithOutput(this SqlCommand command, string name, SqlDbType type = default, int size = default) =>
		//	command.WithOutput<object>(name, type, size);

		//public static SqlCommand WithOutput<T>(this SqlCommand command, string name, SqlDbType type = default, int size = default) =>
		//	command.WithParameter(name, default(T), ParameterDirection.Output, type, size);

		//public static SqlCommand WithInputOutput<T>(this SqlCommand command, string name, T value, SqlDbType type, int size = default) =>
		//	command.WithParameter(name, value, ParameterDirection.InputOutput, type, size);
	}
}