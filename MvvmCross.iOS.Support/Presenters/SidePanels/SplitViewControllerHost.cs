namespace MvvmCross.iOS.Support.Presenters.SidePanels
{
    using UIKit;

    /// <summary>
    /// A wrapper UIViewController to present a UISplitViewController view nested
    /// in a navigation stack instead of being the root application view controller
    /// class
    /// </summary>
    /// <seealso cref="UIKit.UIViewController" />
    [PanelPresentation(PanelEnum.Center, PanelHintType.ActivePanel, true)]
    public class SplitViewControllerHost : UIViewController
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