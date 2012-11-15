//using Android.Content;
//using Android.Views;

using Foobar.Dialog.Core.Lists;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Dialog.Touch.AutoView.Interfaces.Lists
{
    public interface IMvxLayoutListItemViewFactory
        : IListItemLayout
    {
        UITableViewCell BuildView(NSIndexPath indexPath, object item, string cellId);
    }
}