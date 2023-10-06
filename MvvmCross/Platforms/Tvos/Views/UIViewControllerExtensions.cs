// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using MvvmCross.Logging;
using UIKit;

namespace MvvmCross.Platforms.Tvos.Views
{
    public static class UIViewControllerExtensions
    {
        public static IMvxTvosView GetIMvxTvosView(this UIViewController viewController)
        {
            var mvxView = viewController as IMvxTvosView;
            if (mvxView == null)
            {
                MvxLogHost.Default?.Log(
                    LogLevel.Warning, "Could not get IMvxIosView from ViewController {viewControllerName}", viewController.GetType().Name);
            }
            return mvxView;
        }
    }
}
