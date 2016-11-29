using System.Collections.Generic;

namespace MvvmCross.iOS.Support.Views.Expandable
{
	class AccordionSectionExpandableController : SectionExpandableController
	{
		public override IEnumerable<int> ToggleState(int atIndex)
		{
			foreach (var currentlyExpandedSections in ExpandedIndexesSet)
				yield return currentlyExpandedSections;

			bool isIndexExpanded = IsExpanded(atIndex);
			ExpandedIndexesSet.Clear();

			if (!isIndexExpanded)
			{
				ExpandedIndexesSet.Add(atIndex);
				yield return atIndex;
			}
		}
	}
}