#region Copyright

// <copyright file="IElementSizing.cs" company="Cirrious">
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
    ///   This interface is implemented by Element classes that will have
    ///   different heights
    /// </summary>
    public interface IElementSizing
    {
        float GetHeight(UITableView tableView, NSIndexPath indexPath);
    }
}