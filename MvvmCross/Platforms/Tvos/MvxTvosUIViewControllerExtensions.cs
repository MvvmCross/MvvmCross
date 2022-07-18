// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using UIKit;

namespace MvvmCross.Platforms.Tvos
{
    public static class MvxTvosUIViewControllerExtensions
    {
        public static bool IsVisible(this UIViewController controller)
        {
            // based from answer to http://stackoverflow.com/questions/2777438/how-to-tell-if-uiviewcontrollers-view-is-visible
            // would ideally prefer to use ViewWillAppear in the controller code - but UINavigationController doesn't pass on
            // these messages correctly
            if (!controller.IsViewLoaded)
                return false;

            var uiNavigationParent = controller.ParentViewController as UINavigationController;
            if (uiNavigationParent == null)
            {
                return controller.View.Window != null;
            }
            else
            {
                return Equals(uiNavigationParent.VisibleViewController, controller);
            }
        }
    }
}
