﻿using MvvmCross.Platform.Logging;
using UIKit;

namespace MvvmCross.iOS.Views
{
    public static class UIViewControllerExtensionMethods
    {
        public static IMvxIosView GetIMvxIosView(this UIViewController viewController)
        {
            var mvxView = viewController as IMvxIosView;
            if (mvxView == null)
            {
                MvxLog.Instance.Warn($"Could not get IMvxIosView from ViewController {viewController.GetType().Name}");
            }
            return mvxView;
        }
    }
}
