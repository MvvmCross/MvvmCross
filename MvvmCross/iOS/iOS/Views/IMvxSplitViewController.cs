using System;
using UIKit;
namespace MvvmCross.iOS.Views
{
    public interface IMvxSplitViewController
    {
        void ShowMasterView(UIViewController viewController);

        void ShowDetailView(UIViewController viewController);
    }
}
