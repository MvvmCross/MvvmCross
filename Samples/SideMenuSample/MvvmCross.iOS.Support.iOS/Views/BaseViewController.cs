namespace MvvmCross.iOS.Support.iOS.Views
{
    using MvvmCross.Core.ViewModels;
    using Core.ViewModels;
    using MvvmCross.iOS.Views;
    using UIKit;

    public class BaseViewController<TViewModel> : MvxViewController<TViewModel> where TViewModel : class, IMvxViewModel
    {
        /// <summary>
        /// Called after the controller’s <see cref="P:UIKit.UIViewController.View"/> is loaded into memory.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is called after <c>this</c> <see cref="T:UIKit.UIViewController"/>'s <see cref="P:UIKit.UIViewController.View"/> and its entire view hierarchy have been loaded into memory. This method is called whether the <see cref="T:UIKit.UIView"/> was loaded from a .xib file or programmatically.
        /// </para>
        /// </remarks>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.Gray;

            EdgesForExtendedLayout = UIRectEdge.None;
        }
    }
}