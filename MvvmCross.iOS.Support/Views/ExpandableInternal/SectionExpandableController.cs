using System.Collections.Generic;

namespace MvvmCross.iOS.Support.Views.ExpandableInternal
{
	internal abstract class SectionExpandableController
	{
		protected readonly HashSet<int> ExpandedIndexesSet = new HashSet<int>();

		/// <summary>
		/// Toggles expandable state and returns what indexes had changed.
		/// </summary>
		/// <param name="atIndex"></param>
		public abstract IEnumerable<int> ToggleState(int atIndex);

		public bool IsExpanded(int atIndex) => ExpandedIndexesSet.Contains(atIndex);

		public void ResetState() => ExpandedIndexesSet.Clear();
	}
}