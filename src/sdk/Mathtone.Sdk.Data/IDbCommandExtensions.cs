using System.Data;
using System.Data.Common;

namespace Mathtone.Sdk.Data {
	public static class IDbCommandExtensions {
		public static CMD WithTemplate<CMD>(this CMD command, string tag, string value, bool autoEscape = true)
			where CMD : IDbCommand {

			var v = autoEscape ? value.Replace("'", "''") : value;
			command.CommandText = command.CommandText.Replace(tag, v);
			return command;
		}

		public static CMD WithInput<CMD, T>(this CMD command, string name, T value, int size = default)
			where CMD : IDbCommand =>
			command.WithParameter(name, value, ParameterDirection.Input, size);

		public static CMD WithInputOutput<CMD, T>(this CMD command, string name, T value, int size = default)
			where CMD : IDbCommand =>
			command.WithParameter(name, value, ParameterDirection.InputOutput, size);

		public static CMD WithParameter<CMD, T>(this CMD command, string name, T value, ParameterDirection direction, int size = default)
			where CMD : IDbCommand {

			var p = command.CreateParameter();
			p.ParameterName = name;
			p.Value = value;
			p.Direction = direction;
			p.Size = size;
			return command.WithParameter(p);
		}

		public static CMD WithParameter<CMD, P>(this CMD command, P parameter)
			where CMD : IDbCommand
			where P : IDbDataParameter {
			command.Parameters.Add(parameter);
			return command;
		}
	}

	public static class DbCommandExtensions {
		public static CMD WithParameter<CMD, T>(this CMD command, string name, T value, ParameterDirection direction, DbType type, int size = default)
			where CMD : DbCommand {
			var p = command.CreateParameter();
			p.ParameterName = name;
			p.Value = value;
			p.Direction = direction;
			p.Size = size;
			p.DbType = type;
			return command.WithParameter(p);
		}

		public static CMD WithInput<CMD, T>(this CMD command, string name, T value, DbType type, int size = default)
			where CMD : DbCommand =>
			command.WithParameter(name, value, ParameterDirection.Input, type, size);
	}
}