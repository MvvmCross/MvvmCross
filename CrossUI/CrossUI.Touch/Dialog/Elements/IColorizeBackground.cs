using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements
{
    /// <summary>
    ///   This interface is implemented by Elements that needs to update
    ///   their cells Background properties just before they are displayed
    ///   to the user.   This is an iOS 3 requirement to properly render
    ///   a cell.
    /// </summary>
    public interface IColorizeBackground {
        void WillDisplay (UITableView tableView, UITableViewCell cell, NSIndexPath indexPath);
    }
}