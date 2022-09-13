using Mathtone.Sdk.Data;
using Mathtone.Sdk.Data.Sql;
using Moq;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata;

namespace Sandbox.Tests {

	

	public class UnitTest1 {

		[Fact]
		public void AddSqlInputParam() {
			var p = new SqlCommand().WithInput("@A", 0, SqlDbType.Variant).Parameters["@A"];
			Assert.Equal(ParameterDirection.Input, p.Direction);
		}

		[Fact]
		public void AddInputParam() {
			var p = new SqlCommand().WithInput("@A", 0).Parameters["@A"];
			Assert.Equal(ParameterDirection.Input, p.Direction);
		}

		[Fact]
		public void CreateCommand() {
			var text = "SELECT * FROM TEST";
			var cmd = new SqlConnection().CreateCommand(text);
			Assert.Equal(text, cmd.CommandText);
		}

		[Fact]
		public void ConsumeToArray() {
			var data = new[]{
				new {
					TextValue="A",
					IntValue = 0
				},
				new {
					TextValue="B",
					IntValue = 1,
				},
				new {
					TextValue="C",
					IntValue = 2,
				}
			};

			var array = DataReader.Create(data).ToArray(r => $"{r["TextValue"]}:{r["IntValue"]}");
			Assert.Equal(new[] { "A:0", "B:1","C:2" }, array);
		}

		[Fact]
		public void DataRecordFieldValues() {
			var data = new[]{
				new {
					TextValue="A",
					IntValue = 0
				}
			};

			var reader = DataReader.Create(data);
			Assert.True(reader.Read());
			Assert.Equal(0, reader.Field<int>("IntValue"));
			Assert.Equal(0, reader.Field<int>(1));
			Assert.Equal("0", reader.Field("IntValue"));
			Assert.Equal("0", reader.Field(1));
		}
	}

	public static class DataReader {
		public static TestDataReader<T> Create<T>(IEnumerable<T> data) => new(data);
	}

	public class TestParameterCollection : DbParameterCollection {
		public override int Add(object value) {
			throw new NotImplementedException();
		}

		public override void AddRange(Array values) {
			throw new NotImplementedException();
		}

		public override void Clear() {
			throw new NotImplementedException();
		}

		public override bool Contains(object value) {
			throw new NotImplementedException();
		}

		public override bool Contains(string value) {
			throw new NotImplementedException();
		}

		public override void CopyTo(Array array, int index) {
			throw new NotImplementedException();
		}

		public override IEnumerator GetEnumerator() {
			throw new NotImplementedException();
		}

		protected override DbParameter GetParameter(int index) {
			throw new NotImplementedException();
		}

		protected override DbParameter GetParameter(string parameterName) {
			throw new NotImplementedException();
		}

		public override int IndexOf(object value) {
			throw new NotImplementedException();
		}

		public override int IndexOf(string parameterName) {
			throw new NotImplementedException();
		}

		public override void Insert(int index, object value) {
			throw new NotImplementedException();
		}

		public override void Remove(object value) {
			throw new NotImplementedException();
		}

		public override void RemoveAt(int index) {
			throw new NotImplementedException();
		}

		public override void RemoveAt(string parameterName) {
			throw new NotImplementedException();
		}

		protected override void SetParameter(int index, DbParameter value) {
			throw new NotImplementedException();
		}

		protected override void SetParameter(string parameterName, DbParameter value) {
			throw new NotImplementedException();
		}

		public override int Count { get; }
		public override object SyncRoot { get; }
	}

	public class TestCommand : IDbCommand {
		public void Cancel() {
			throw new NotImplementedException();
		}

		public IDbDataParameter CreateParameter() {
			throw new NotImplementedException();
		}

		public int ExecuteNonQuery() {
			throw new NotImplementedException();
		}

		public IDataReader ExecuteReader() {
			throw new NotImplementedException();
		}

		public IDataReader ExecuteReader(CommandBehavior behavior) {
			throw new NotImplementedException();
		}

		public object? ExecuteScalar() {
			throw new NotImplementedException();
		}

		public void Prepare() {
			throw new NotImplementedException();
		}

		public string CommandText { get; set; }=String.Empty;
		public int CommandTimeout { get; set; }
		public CommandType CommandType { get; set; }
		public IDbConnection? Connection { get; set; }
		public IDataParameterCollection Parameters { get; }
		public IDbTransaction? Transaction { get; set; }
		public UpdateRowSource UpdatedRowSource { get; set; }

		public void Dispose() {
			throw new NotImplementedException();
		}
	}

	public class TestDataReader<T> : IDataReader {

		readonly IEnumerable<T> _data;
		IEnumerator<T>? _enumerator;

#pragma warning disable S2743 // Static fields should not be used in generic types
		private static readonly Dictionary<string, Delegate> _nameMap = new();
		private static readonly List<Delegate> _idxMap = new();
#pragma warning restore S2743 // Static fields should not be used in generic types


		static TestDataReader() {
			var type = typeof(T);
			var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToArray();

			foreach (var p in props) {
				var x = Expression.Parameter(type, "x");
				var ex = DynamicExpressionParser.ParseLambda(new ParameterExpression[] { x }, p.PropertyType, $"x.{p.Name}").Compile();

				_nameMap.Add(p.Name, ex);
				_idxMap.Add(ex);
			}
		}

		public TestDataReader(IEnumerable<T> data) {
			_data = data;
			_enumerator = _data.GetEnumerator();
		}

		public void Close() {
			_enumerator?.Dispose();
			_enumerator = default;
		}

		public DataTable? GetSchemaTable() {
			throw new NotImplementedException();
		}

		public bool NextResult() {
			throw new NotImplementedException();
		}

		public bool Read() => _enumerator!.MoveNext();

		public int Depth { get; }
		public bool IsClosed => _enumerator == null;
		public int RecordsAffected => _data.Count();

		public bool GetBoolean(int i) => Convert.ToBoolean(this[i]);

		public byte GetByte(int i) => Convert.ToByte(this[i]);

		public long GetBytes(int i, long fieldOffset, byte[]? buffer, int bufferoffset, int length) {
			throw new NotImplementedException();
		}

		public long GetChars(int i, long fieldoffset, char[]? buffer, int bufferoffset, int length) {
			throw new NotImplementedException();
		}

		public IDataReader GetData(int i) {
			throw new NotImplementedException();
		}

		public string GetDataTypeName(int i) {
			throw new NotImplementedException();
		}

		public char GetChar(int i) => Convert.ToChar(this[i]);

		public DateTime GetDateTime(int i) => Convert.ToDateTime(this[i]);

		public decimal GetDecimal(int i) => Convert.ToDecimal(this[i]);

		public double GetDouble(int i) => Convert.ToDouble(this[i]);

		[return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)]
		public Type GetFieldType(int i) {
			throw new NotImplementedException();
		}

		public float GetFloat(int i) => Convert.ToSingle(this[i]);

		public Guid GetGuid(int i) => (Guid)this[i];

		public short GetInt16(int i) => Convert.ToInt16(this[i]);

		public int GetInt32(int i) => Convert.ToInt32(this[i]);

		public long GetInt64(int i) => Convert.ToInt64(this[i]);

		public string GetName(int i) {
			throw new NotImplementedException();
		}

		public int GetOrdinal(string name) {
			throw new NotImplementedException();
		}

		public string GetString(int i) => Convert.ToString(this[i])!;

		public object GetValue(int i) => this[i];

		public int GetValues(object[] values) {
			throw new NotImplementedException();
		}

		public bool IsDBNull(int i) => this[i] == null || this[i] == DBNull.Value;

		public int FieldCount => _idxMap.Count;

		public object this[int i] => _idxMap[i].DynamicInvoke(_enumerator!.Current)!;
		public object this[string name] => _nameMap[name].DynamicInvoke(_enumerator!.Current)!;

		private bool disposedValue;

		protected virtual void Dispose(bool disposing) {
			if (!disposedValue) {
				if (disposing) {
					Close();
				}
				disposedValue = true;
			}
		}
		public void Dispose() {
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}