using Cirrious.MvvmCross.Touch.Views.Presenters;
using MonoTouch.UIKit;

namespace TwitterSearch.UI.Touch
{
    public class TwitterPhoneSearchPresenter 
        : MvxModalSupportTouchViewPresenter
	{
        public TwitterPhoneSearchPresenter(UIApplicationDelegate applicationDelegate, UIWindow window)
			: base(applicationDelegate, window)
		{
		}
		
		protected override UINavigationController CreateNavigationController (UIViewController viewController)
		{
			var toReturn = base.CreateNavigationController (viewController);
			toReturn.NavigationBarHidden = false;
			return toReturn;
		}
	}
}

