using System.Collections.Generic;

namespace MvvmCross.iOS.Support.ExpandableTableView.Core
{
	public class KittenGroup : List<Kitten>
	{
		public string Title { get; set; }

		public KittenGroup(IEnumerable<Kitten> collection) : base(collection)
		{

		}
	}
}
