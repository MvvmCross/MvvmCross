using System.Collections.Generic;

namespace MvvmCross.iOS.Support.ExpandableTableView.Core
{
    public class KittenGroup : List<Kitten>
    {
        public KittenGroup(IEnumerable<Kitten> collection) : base(collection)
        {
        }

        public string Title { get; set; }
    }
}