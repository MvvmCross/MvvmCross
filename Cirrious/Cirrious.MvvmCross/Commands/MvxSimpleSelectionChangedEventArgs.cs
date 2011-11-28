using System.Collections;

namespace Cirrious.MvvmCross.Commands
{
    public class MvxSimpleSelectionChangedEventArgs
    {
        public IList AddedItems { get; set;  }
        public IList RemovedItems { get; set; }
    }
}
