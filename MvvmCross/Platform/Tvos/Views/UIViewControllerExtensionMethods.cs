using System;

using MvvmCross.Platform.Platform;

using UIKit;

namespace MvvmCross.tvOS.Views
{
    public static class UIViewControllerExtensionMethods
    {
        public static IMvxTvosView GetIMvxTvosView(this UIViewController viewController)
        {
            var mvxView = viewController as IMvxTvosView;
            if (mvxView == null)
            {
                MvxTrace.Warning($"Could not get IMvxIosView from ViewController {viewController.GetType().Name}");
            }
            return mvxView;
        }
    }
}
