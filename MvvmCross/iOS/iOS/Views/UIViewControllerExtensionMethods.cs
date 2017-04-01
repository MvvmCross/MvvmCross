using System;
using MvvmCross.Platform.Exceptions;
using UIKit;
using MvvmCross.Platform.Platform;

namespace MvvmCross.iOS.Views
{
    public static class UIViewControllerExtensionMethods
    {
        public static IMvxIosView GetIMvxIosView(this UIViewController viewController)
        {
            var mvxView = viewController as IMvxIosView;
            if(mvxView == null)
            {
                MvxTrace.Warning("Could not get IMvxIosView from ViewController!");
            }
            return mvxView;
        }
    }
}
