using System.Drawing;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.UIKit;

namespace TwitterSearch.UI.Touch.Views
{
    public sealed class SplitViewController : UISplitViewController
	{
        public SplitViewController ()
		{
			View.Bounds = new RectangleF(0,0,UIScreen.MainScreen.Bounds.Width,UIScreen.MainScreen.Bounds.Height);
			Delegate = new SplitViewDelegate();

            this.ViewControllers = new UIViewController[] { new EmptyViewController(), new EmptyViewController() };
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
        {
            return true;
        }

	    public void SetPrimaryView(IMvxTouchView view)
	    {
	        var controller = view as UIViewController;
            this.ViewControllers = new UIViewController[] { controller, ViewControllers[1] };
        }

	    public void SetSecondaryView(IMvxTouchView view)
	    {
            var controller = view as UIViewController;
            this.ViewControllers = new UIViewController[] { ViewControllers[0], controller };
        }
	}

 	public class SplitViewDelegate : UISplitViewControllerDelegate
    {
        public override bool ShouldHideViewController (UISplitViewController svc, UIViewController viewController, UIInterfaceOrientation inOrientation)
        {
            return false;
        }
	}
}