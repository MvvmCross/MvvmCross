using System.Collections.Generic;
using System.Linq;

namespace MvvmCross.iOS.Support.Views.Expandable.Controllers
{
	class AccordionSectionExpandableController : SectionExpandableController
	{
		public override ToggleExpandStateResponse ToggleState(int atIndex)
		{
			var collapsedIndexes = ExpandedIndexesSet.ToList();
			var expandedIndexes = new List<int>();
  
			bool isIndexExpanded = IsExpanded(atIndex);
			ResetState();

			if (!isIndexExpanded)
			{
				ExpandedIndexesSet.Add(atIndex);
				expandedIndexes.Add(atIndex);
			}

			return new ToggleExpandStateResponse(expandedIndexes, collapsedIndexes);
		}
	}
}