// IElementSizing.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Foundation;
using System;
using UIKit;

namespace CrossUI.Touch.Dialog.Elements
{
    /// <summary>
    ///   This interface is implemented by Element classes that will have
    ///   different heights
    /// </summary>
    public interface IElementSizing
    {
        nfloat GetHeight(UITableView tableView, NSIndexPath indexPath);
    }
}