using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements
{
    /// <summary>
    ///   This interface is implemented by Element classes that will have
    ///   different heights
    /// </summary>
    public interface IElementSizing {
        float GetHeight (UITableView tableView, NSIndexPath indexPath);
    }
}