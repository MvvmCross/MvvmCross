//using Android.Content;
//using Android.Views;

using CrossUI.Core.Elements.Lists;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.AutoView.Touch.Interfaces.Lists
{
    public interface IMvxLayoutListItemViewFactory
        : IListItemLayout
    {
        UITableViewCell BuildView(NSIndexPath indexPath, object item, string cellId);
    }
}