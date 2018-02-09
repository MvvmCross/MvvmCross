// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Logging;
using UIKit;

namespace MvvmCross.Platform.Ios.Views
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
