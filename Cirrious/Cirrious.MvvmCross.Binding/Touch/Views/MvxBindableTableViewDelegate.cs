using System;
using System.Collections;
using Cirrious.MvvmCross.Commands;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public class MvxBindableTableViewDelegate : UITableViewDelegate
    {
        public IList ItemsSource { get; set; }
        public event EventHandler<MvxSimpleSelectionChangedEventArgs> SelectionChanged;

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (ItemsSource == null)
                return;

            var item = ItemsSource[indexPath.Row];
            var selectionChangedArgs = MvxSimpleSelectionChangedEventArgs.JustAddOneItem(item);

            var handler = SelectionChanged;
            if (handler != null)
                handler(this, selectionChangedArgs);
        }
    }
}