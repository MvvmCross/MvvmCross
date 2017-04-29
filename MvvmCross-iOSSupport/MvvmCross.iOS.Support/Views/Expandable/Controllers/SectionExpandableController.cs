using System.Collections.Generic;
using System.Linq;

namespace MvvmCross.iOS.Support.Views.Expandable.Controllers
{
    internal abstract class SectionExpandableController
    {
        protected readonly HashSet<int> ExpandedIndexesSet = new HashSet<int>();

        public IEnumerable<int> ExpandedIndexes => ExpandedIndexesSet.ToList();

        /// <summary>
        ///     Toggles expandable state and returns what indexes had changed.
        /// </summary>
        /// <param name="atIndex"></param>
        public abstract ToggleExpandStateResponse ToggleState(int atIndex);

        public bool IsExpanded(int atIndex)
        {
            return ExpandedIndexesSet.Contains(atIndex);
        }

        public void ResetState()
        {
            ExpandedIndexesSet.Clear();
        }
    }
}