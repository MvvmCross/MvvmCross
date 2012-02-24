using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public class MvxDefaultBindableTableViewSource : MvxBindableTableViewSource
    {
        static readonly NSString CellIdentifier = new NSString("MvxDefaultBindableTableViewCell");
        
        public MvxDefaultBindableTableViewSource(UITableView tableView) : base(tableView)
        {
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            var reuse = tableView.DequeueReusableCell(CellIdentifier);
            if (reuse != null)
                return reuse;

            return new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);
        }
    }
}