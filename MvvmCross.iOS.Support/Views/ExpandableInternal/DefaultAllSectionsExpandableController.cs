using System.Collections.Generic;

namespace MvvmCross.iOS.Support.Views.ExpandableInternal
{
	internal class DefaultAllSectionsExpandableController : SectionExpandableController
	{
		public override IEnumerable<int> ToggleState(int atIndex)
		{
			if (ExpandedIndexesSet.Contains(atIndex))
				ExpandedIndexesSet.Remove(atIndex);
			else
				ExpandedIndexesSet.Add(atIndex);

			yield return atIndex;
		}

	
	}
}