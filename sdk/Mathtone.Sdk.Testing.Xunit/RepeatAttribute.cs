using System.Reflection;
using Xunit.Sdk;

namespace Mathtone.Sdk.Testing.Xunit {
	//public sealed class RepeatAttribute : DataAttribute {
	//	private readonly int count;

	//	public RepeatAttribute(int count) {
	//		if (count < 1) {
	//			throw new System.ArgumentOutOfRangeException(
	//				paramName: nameof(count),
	//				message: "Repeat count must be greater than 0."
	//				);
	//		}
	//		this.count = count;
	//	}

	//	public override IEnumerable<object[]> GetData(MethodInfo testMethod) {
	//		foreach (var I in Enumerable.Range(1, count)) {
	//			yield return new object[] { I };
	//		}
	//	}
	//}
}