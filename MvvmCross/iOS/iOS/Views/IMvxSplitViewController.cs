using System;
using UIKit;
namespace MvvmCross.iOS.Views
{
    public interface IMvxSplitViewController
    {
        void ShowMasterView(UIViewController viewController, bool wrapInNavigationController);

        void ShowDetailView(UIViewController viewController, bool wrapInNavigationController);
    }
}
