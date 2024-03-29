﻿using Mathtone.Sdk.Data.Tests;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using System.Data.Common;
using Xunit.Abstractions;

namespace Mathtone.Sdk.Data.Npgsql.Tests {

	public class NpgsqlCommandTests : DbCommandTestBase<NpgsqlConnection, NpgsqlCommand> {

		public NpgsqlCommandTests(ITestOutputHelper output) : base(output) { }

		protected override NpgsqlConnection Connect() => new(DB.ConnectionString);

		[Fact]
		public void WithInput_SqlDbType() {
			var p = Connect().CreateCommand("").WithInput("@id", 1, NpgsqlDbType.Integer).Parameters["@id"];
			Assert.Equal(1, p.Value);
			Assert.Equal(ParameterDirection.Input, p.Direction);
		}

		[Fact]
		public async Task ExecuteResult() {
			var rslt = await Connect().UsedAsync(cn => cn
				.TextCommand("SELECT 'Hello'")
				.ExecuteResult(cmd => cmd.ExecuteScalarAsync(), (cmd, rslt) => rslt!.ToString())
			);
			Assert.Equal("Hello", rslt);
		}

		//[Fact]
		//public async Task Used() {
		//	var rslt = Connect().Used(cn => cn
		//		.TextCommand("SELECT 'Hello'")
		//		.ExecuteResult(cmd => cmd.ExecuteScalar(), (cmd, rslt) => rslt!.ToString())
		//	);
		//	Assert.Equal("Hello", rslt);
		//}
	}
}