// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;

namespace MvvmCross.Platforms.Ios.Views.Expandable.Controllers
{
    internal class ToggleExpandStateResponse
    {
        public ToggleExpandStateResponse(IEnumerable<int> expandedIndexes, IEnumerable<int> collapsedIndexes)
        {
            ExpandedIndexes = expandedIndexes;
            CollapsedIndexes = collapsedIndexes;
        }

        public IEnumerable<int> ExpandedIndexes { get; }

        public IEnumerable<int> CollapsedIndexes { get; }

        public IEnumerable<int> AllModifiedIndexes => ExpandedIndexes.Concat(CollapsedIndexes).ToList();
    }
}
