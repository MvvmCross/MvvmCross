// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;

namespace MvvmCross.Platforms.Ios.Views.Expandable.Controllers
{
    internal class AccordionSectionExpandableController : SectionExpandableController
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
