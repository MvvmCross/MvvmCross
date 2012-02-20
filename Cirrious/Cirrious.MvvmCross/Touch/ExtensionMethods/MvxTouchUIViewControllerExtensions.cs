using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Touch.ExtensionMethods
{
    public static class MvxTouchUIViewControllerExtensions
    {
        public static bool IsVisible(this UIViewController controller)
        {
#warning What is this IsVisible list			
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
				return uiNavigationParent.VisibleViewController == controller;
			}
            //return controller.IsViewLoaded && controller.View.Window != null; 
		}
    }
}