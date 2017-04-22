using MvvmCross.Platform.Platform;
using UIKit;

namespace MvvmCross.iOS.Views
{
    public static class UIViewControllerExtensionMethods
    {
        public static IMvxIosView GetIMvxIosView(this UIViewController viewController)
        {
            var mvxView = viewController as IMvxIosView;
            if(mvxView == null)
            {
                MvxTrace.Warning($"Could not get IMvxIosView from ViewController {viewController.GetType().Name}");
            }
            return mvxView;
        }
    }
}
