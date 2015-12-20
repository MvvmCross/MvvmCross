// IMvxLayoutListItemViewFactory.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Touch.Interfaces.Lists
{
    using CrossUI.Core.Elements.Lists;

    using Foundation;

    using UIKit;

    public interface IMvxLayoutListItemViewFactory
        : IListItemLayout
    {
        UITableViewCell BuildView(NSIndexPath indexPath, object item, string cellId);
    }
}