using System;
using MvvmCross.Platform.Logging;
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
                MvxLog.Instance.Warn($"Could not get IMvxIosView from ViewController {viewController.GetType().Name}");
            }
            return mvxView;
        }
    }
}
