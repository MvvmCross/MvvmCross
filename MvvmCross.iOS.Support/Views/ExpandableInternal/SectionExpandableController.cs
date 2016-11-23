using System.Collections.Generic;

namespace MvvmCross.iOS.Support.Views.ExpandableInternal
{
	internal class SectionExpandableController
	{
		private readonly HashSet<int> _expandedIndexesSet = new HashSet<int>();

		public SectionExpandableController()
		{
			
		}

		/// <summary>
		/// Toggles expandable state and returns what indexes had changed.
		/// </summary>
		/// <param name="atIndex"></param>
		public IEnumerable<int> ToggleState(int atIndex)
		{
			if (_expandedIndexesSet.Contains(atIndex))
				_expandedIndexesSet.Remove(atIndex);
			else
				_expandedIndexesSet.Add(atIndex);

			yield return atIndex;
		}

		public bool IsExpanded(int atIndex) => _expandedIndexesSet.Contains(atIndex);

		public void ResetState() => _expandedIndexesSet.Clear();
	}
}