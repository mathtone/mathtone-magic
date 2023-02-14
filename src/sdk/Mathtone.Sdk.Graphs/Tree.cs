using System.Collections;
using System.Linq;

namespace Mathtone.Sdk.Graphs {

	public class Tangle<T> : IEnumerable<Tangle<T>> {

		//Tangle<T>? _parent;
		readonly List<Tangle<T>> _children = new();
		readonly IList<Tangle<T>> _parents = new List<Tangle<T>>();

		public T? Value { get; set; }
		public IList<Tangle<T>> Parents => _parents;
		public IList<Tangle<T>> Children => _children;

		public Tangle() : this(default) { }
		public Tangle(T? value) => Value = value;


		public void AddParent(Tangle<T> parent) {
			parent._children.Add(this);
			Parents.Add(parent);
		}

		public void RemoveParent(Tangle<T> parent) {
			parent._children.Remove(this);
			Parents.Remove(parent);
		}

		public void Remove(Tangle<T> child) {
			_children.Remove(child);
			child.Parents.Remove(this);
		}
		public void Add(Tangle<T> child) {
			_children.Add(child);
			child.Parents.Add(this);
		}

		public IEnumerable<Tangle<T>> WithAncestors => Ancestors.Prepend(this);
		public IEnumerable<Tangle<T>> Ancestors {
			get {
				foreach (var c in this.Parents) {
					yield return c;
					foreach (var d in c.Ancestors) {
						yield return d;
					}
				}
			}
		}
		public IEnumerable<Tangle<T>> WithDescendents => Descendents.Prepend(this);
		public IEnumerable<Tangle<T>> Descendents {
			get {
				foreach (var c in this) {
					yield return c;
					foreach (var d in c.Descendents) {
						yield return d;
					}
				}
			}
		}

		public IEnumerator<Tangle<T>> GetEnumerator() => _children.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


		public Tangle<T> Clone() {
			var t = new Tangle<T>(Value);
			foreach (var c in this.Children) {
				t.Add(c.Clone());
			}
			foreach (var p in this.Parents) {
				t.AddParent(p.Clone());
			}
			return t;
		}
	}

	public class Tree<T> : IEnumerable<Tree<T>>, ICloneable {

		readonly List<Tree<T>> _children = new();
		Tree<T>? _parent;

		public T? Value { get; set; }
		public Tree<T>? Parent { get => _parent; set => SetParent(value); }
		public Tree<T> this[int index] => _children[index];

		public Tree() : this(default) { }
		public Tree(T? value) => Value = value;

		public void SetParent(Tree<T>? parent) {
			_parent?._children.Remove(this);
			_parent = parent;
			_parent?._children.Add(this);
		}

		public void Add(Tree<T> child) =>
			child.SetParent(this);

		public IEnumerable<Tree<T>> WithAncestors => Ancestors.Prepend(this);
		public IEnumerable<Tree<T>> Ancestors {
			get {
				var p = Parent;
				while (p != null) {
					yield return p;
					p = p.Parent;
				}
			}
		}
		public IEnumerable<Tree<T>> WithDescendents => Descendents.Prepend(this);
		public IEnumerable<Tree<T>> Descendents {
			get {
				foreach (var c in this) {
					yield return c;
					foreach (var d in c.Descendents) {
						yield return d;
					}
				}
			}
		}

		public IEnumerator<Tree<T>> GetEnumerator() => _children.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		
		public Tree<T> Clone() {
			var t = new Tree<T>(Value);
			foreach (var c in this) {
				t.Add(c.Clone());
			}
			return t;
		}

		object ICloneable.Clone() => Clone();
	}
}