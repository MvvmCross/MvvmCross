// MvxTouchUIViewControllerExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using UIKit;

namespace Cirrious.CrossCore.Touch
{
    public static class MvxTouchUIViewControllerExtensions
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