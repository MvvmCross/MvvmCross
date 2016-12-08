using System.Collections.Generic;

namespace MvvmCross.iOS.Support.Views.Expandable.Controllers
{
	internal class DefaultAllSectionsExpandableController : SectionExpandableController
	{
		public override ToggleExpandStateResponse ToggleState(int atIndex)
		{
			List<int> collapsedIndexes = new List<int>();
			List<int> expandedIndexes = new List<int>();
			if (ExpandedIndexesSet.Contains(atIndex))
			{
				ExpandedIndexesSet.Remove(atIndex);
				collapsedIndexes.Add(atIndex);
			}
			else
			{
				ExpandedIndexesSet.Add(atIndex);
				expandedIndexes.Add(atIndex);
			}

			return new ToggleExpandStateResponse(expandedIndexes, collapsedIndexes);
		}

	
	}
}