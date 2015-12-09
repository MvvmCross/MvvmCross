// IMvxLayoutListItemViewFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using CrossUI.Core.Elements.Lists;
using Foundation;
using UIKit;

namespace Cirrious.MvvmCross.AutoView.Touch.Interfaces.Lists
{
    public interface IMvxLayoutListItemViewFactory
        : IListItemLayout
    {
        UITableViewCell BuildView(NSIndexPath indexPath, object item, string cellId);
    }
}