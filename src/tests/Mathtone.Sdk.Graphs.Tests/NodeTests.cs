using System.Collections;

namespace Mathtone.Sdk.Graphs.Tests {
	public class NodeTests {
		[Fact]
		public void MakeNode() {

		}
	}



	//public class MatrixGraph : GraphBase {
	//	int[,] _matrix;
	//	public MatrixGraph(int numVertices, bool directed = false) : base(numVertices, directed) {
	//		_matrix = GenerateEmptyMatrix(numVertices);
	//	}

	//	private int[,] GenerateEmptyMatrix(int numVertices) {
	//		var rtn = new int[numVertices, numVertices];
	//		for (int row = 0; row < numVertices; row++) {
	//			for (int col = 0; col < numVertices; col++) {
	//				_matrix[row, col] = 0;
	//			}
	//		}

	//		return rtn;
	//	}

	//	public override IEnumerable<int> GetAdjacentVertices(int v) {
	//		if (v < 0 || v >= NumVertices)
	//			throw new ArgumentOutOfRangeException(nameof(v));

	//		List<int> adjacentVertices = new List<int>();
	//		for (int i = 0; i < this.NumVertices; i++) {
	//			if (_matrix[v, i] > 0)
	//				adjacentVertices.Add(i);
	//		}
	//		return adjacentVertices;
	//	}

	//	public override void AddEdge(int v1, int v2, int weight = 1) {
	//		if (v1 >= this.NumVertices || v2 >= this.NumVertices || v1 < 0 || v2 < 0)
	//			throw new ArgumentException("Vertices are out of bounds");

	//		if (weight < 1)
	//			throw new ArgumentException("Weight cannot be less than 1");

	//		_matrix[v1, v2] = weight;

	//		//In an undirected graph all edges are bi-directional
	//		if (!_directed)
	//			_matrix[v2, v1] = weight;
	//	}
	//}


	//public abstract class GraphBase {

	//	protected readonly bool _directed;

	//	public GraphBase(int numVertices, bool directed = false) {
	//		this.NumVertices = numVertices;
	//		this._directed = directed;
	//	}

	//	public int NumVertices { get; protected set; }

	//	public abstract void AddEdge(int v1, int v2, int weight);

	//	public abstract IEnumerable<int> GetAdjacentVertices(int v);

	//	//public abstract int GetEdgeWeight(int v1, int v2);

	//	//public abstract void Display();


	//}
	//public class Node<T> : IEnumerable<Node<T>>{

	//	readonly List<Node<T>> _edges = new();

	//	public T Value { get; set; }

	//	public Node(T value) =>
	//		Value = value;

	//	public void Connect(Node<T> node) {
	//		_edges.Add(node);

	//	}
	//	public void Disonnect(Node<T> node) {
	//		_edges.Remove(node);
	//	}
	//	public IEnumerator<Node<T>> GetEnumerator() => _edges.GetEnumerator();

	//	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	//}
}