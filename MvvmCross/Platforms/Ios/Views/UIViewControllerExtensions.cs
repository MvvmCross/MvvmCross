// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Logging;
using UIKit;

namespace MvvmCross.Platforms.Ios.Views
{
#nullable enable
    public static class UIViewControllerExtensions
    {
        public static IMvxIosView? GetIMvxIosView(this UIViewController? viewController)
        {
            if (viewController is IMvxIosView iosView)
            {
                return iosView;
            }

            MvxLog.Instance?.Warn($"Could not get IMvxIosView from ViewController {viewController?.GetType().Name}");
            return null;
        }
    }
#nullable restore
}
