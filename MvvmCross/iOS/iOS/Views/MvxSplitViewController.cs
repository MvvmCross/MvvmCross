using System;
using System.Linq;
using Foundation;
using MvvmCross.Core.ViewModels;
using UIKit;

namespace MvvmCross.iOS.Views
{
    public class MvxSplitViewController : MvxBaseSplitViewController, IMvxSplitViewController
    {
        public MvxSplitViewController()
        {
        }

        public MvxSplitViewController(IntPtr handle)
            : base(handle)
        {
        }

        protected MvxSplitViewController(string nibName, NSBundle bundle)
            : base(nibName, bundle)
        {
            this.AdaptForBinding();
        }

        public void ShowDetailView(UIViewController viewController)
        {
            var currentRoot = this.ViewControllers.FirstOrDefault() ?? new UIViewController();

            this.ViewControllers = new UIViewController[] { currentRoot, viewController };
        }

        public void ShowMasterView(UIViewController viewController)
        {
            var currentDetail = this.ViewControllers.LastOrDefault() ?? new UIViewController();

            this.ViewControllers = new UIViewController[] { viewController, currentDetail };
        }
    }

    public class MvxSplitViewController<TViewModel> : MvxSplitViewController
        where TViewModel : IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public MvxSplitViewController() : base()
        {
        }

        public MvxSplitViewController(IntPtr handle)
            : base(handle)
        {
        }

        protected MvxSplitViewController(string nibName, NSBundle bundle)
            : base(nibName, bundle)
        {
            this.AdaptForBinding();
        }
    }
}
