using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathtone.Sdk.Data.Tests {
	//Create xunit test class
	public class IDataRecordExtensionsTests {
		[Fact]
		public void Field_String_Name() {
			//Arrange
			var data = new Mock<IDataRecord>();
			data.Setup(x => x["Name"]).Returns("John");

			//Act
			var result = data.Object.Field("Name", x => x?.ToString()!);

			//Assert
			Assert.Equal("John", result);
		}

		[Fact]
		public void Field_String_Index() {
			//Arrange
			var data = new Mock<IDataRecord>();
			data.Setup(x => x[0]).Returns("John");

			//Act
			var result = data.Object.Field(0, x => x?.ToString()!);

			//Assert
			Assert.Equal("John", result);
		}

		[Fact]
		public void Field_Cast_Name() {
			//Arrange
			var data = new Mock<IDataRecord>();
			data.Setup(x => x["Age"]).Returns(25);

			//Act
			var result = data.Object.Field<decimal>("Age");

			//Assert
			Assert.Equal(25.0m, result);
		}

		[Fact]
		public void Field_Cast_Index() {
			//Arrange
			var data = new Mock<IDataRecord>();
			data.Setup(x => x[1]).Returns(25);

			//Act
			var result = data.Object.Field<decimal>(1);

			//Assert
			Assert.Equal(25.0m, result);
		}

		[Fact]
		public void ToNull() {
			Assert.Null(IDataRecordExtensions.ToNull(Convert.DBNull));
		}
	}
}
