using MvvmCross.iOS.Support.XamarinSidebar.Attributes;
using UIKit;

namespace MvvmCross.iOS.Support.XamarinSidebar
{
    /// <summary>
    /// A wrapper UIViewController to present a UISplitViewController view nested
    /// in a navigation stack instead of being the root application view controller
    /// class
    /// </summary>
    /// <seealso cref="UIKit.UIViewController" />
    [MvxSidebarPresentation(MvxPanelEnum.Center, MvxPanelHintType.PushPanel, true)]
    public class MvxSplitViewControllerHost : UIViewController
    {
        /// <summary>
        /// Displays the content UISplitViewController.
        /// </summary>
        /// <param name="content">The content.</param>
        public void DisplayContentController(UISplitViewController content)
        {
            AddChildViewController(content);
            content.View.Frame = View.Frame;
            View.AddSubview(content.View);
            DidMoveToParentViewController(this);
        }
    }
}