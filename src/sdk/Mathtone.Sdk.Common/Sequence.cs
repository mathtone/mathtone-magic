using System.Diagnostics.CodeAnalysis;

namespace Mathtone.Sdk.Common {
	public static class Sequence {

		[SuppressMessage("Blocker Bug", "S2190:Recursion should not be infinite", Justification = "It's a possiblyn infinite sequence")]
		public static IEnumerable<T> Create<T>(T start, Func<T, T> next) {
			yield return start;
			while (true) {
				start = next(start);
				yield return start;
			}
		}

		public static IEnumerable<T> Create<T>(T start, T end, Func<T, T> next) where T : IComparable<T> {
			if (start.CompareTo(end) > 0) {
				throw new ArgumentException("Start must be less than or equal to end.");
			}
			return SequenceIterator(start, end, next);
		}

		private static IEnumerable<T> SequenceIterator<T>(T start, T end, Func<T, T> next) where T : IComparable<T> {
			for (var current = start; current.CompareTo(end) <= 0; current = next(current)) {
				yield return current;
			}
		}
	}
}