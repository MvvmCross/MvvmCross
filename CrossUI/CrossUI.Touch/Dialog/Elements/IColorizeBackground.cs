#region Copyright

// <copyright file="IColorizeBackground.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace CrossUI.Touch.Dialog.Elements
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