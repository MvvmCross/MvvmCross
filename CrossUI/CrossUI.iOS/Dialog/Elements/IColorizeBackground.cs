// IColorizeBackground.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Foundation;
using UIKit;

namespace CrossUI.iOS.Dialog.Elements
{
    /// <summary>
    ///   This interface is implemented by Elements that needs to update
    ///   their cells Background properties just before they are displayed
    ///   to the user.   This is an iOS 3 requirement to properly render
    ///   a cell.
    /// </summary>
    public interface IColorizeBackground
    {
        void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath);
    }
}