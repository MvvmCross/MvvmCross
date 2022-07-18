// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace MvvmCross.Platforms.Ios.Views.Expandable.Controllers
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
