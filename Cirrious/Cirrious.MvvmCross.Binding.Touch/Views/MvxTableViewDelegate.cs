// MvxTableViewDelegate.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
#warning Kill this file
    /*
    public class MvxTableViewDelegate : UITableViewDelegate
    {
        [MvxSetToNullAfterBinding]
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
     * */
}