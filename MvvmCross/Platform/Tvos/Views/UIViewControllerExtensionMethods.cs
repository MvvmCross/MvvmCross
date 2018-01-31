// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

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
