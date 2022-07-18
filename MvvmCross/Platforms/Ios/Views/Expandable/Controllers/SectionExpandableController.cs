// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;

namespace MvvmCross.Platforms.Ios.Views.Expandable.Controllers
{
    internal abstract class SectionExpandableController
    {
        protected readonly HashSet<int> ExpandedIndexesSet = new HashSet<int>();

        /// <summary>
        /// Toggles expandable state and returns what indexes had changed.
        /// </summary>
        /// <param name="atIndex"></param>
        public abstract ToggleExpandStateResponse ToggleState(int atIndex);

        public bool IsExpanded(int atIndex) => ExpandedIndexesSet.Contains(atIndex);

        public void ResetState() => ExpandedIndexesSet.Clear();

        public IEnumerable<int> ExpandedIndexes => ExpandedIndexesSet.ToList();
    }
}
