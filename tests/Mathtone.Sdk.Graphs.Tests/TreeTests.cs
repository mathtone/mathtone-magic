namespace Mathtone.Sdk.Graphs.Tests {
	public class TreeTests {

		readonly Tree<string> _tree = new() {
			new("A") {
				new("A.1"){
					new("A.1.A"),
					new("A.1.B")
				},
				new("A.2"){
					new("A.2.A"),
					new("A.2.B")
				},
				new("A.3") {
					new("A.3.A"),
					new("A.3.B")
				}
			}
		};

		[Fact]
		public void TestTree() {
			Assert.Equal("A.2.B", _tree[0][1][1].Value);
			Assert.Equal("A.3.A", _tree[0][2][0].Value);
		}

		[Fact]
		public void TestSetParent_1() {
			var t1 = new Tree<string>("A");
			var t2 = new Tree<string>("B") {
				Parent = t1
			};

			Assert.Single(t1);
			Assert.Equal(t1, t2.Parent);
		}

		[Fact]
		public void TestSetParent_2() {
			var t1 = new Tree<string>("A");
			var t2 = new Tree<string>("B") {
				Parent = t1
			};
			t2.Parent = null;
			Assert.Empty(t1);
			Assert.Null(t2.Parent);
		}

		[Fact]
		public void TransferParent() {
			var t1 = new Tree<string>("A");
			var t2 = new Tree<string>("B") {
				Parent = t1
			};
			var t3 = new Tree<string>("c");

			Assert.Single(t1);
			Assert.Equal(t1, t2.Parent);

			t3.Add(t2);

			Assert.Equal(t3, t2.Parent);
			Assert.Empty(t1);
		}

		[Fact]
		public void WithDescendents() {
			Assert.Equal(10, _tree[0].WithDescendents.Count());
		}

		[Fact]
		public void WithAncestors() {
			Assert.Equal(4, _tree[0][2][1].WithAncestors.Count());
		}
	}
}