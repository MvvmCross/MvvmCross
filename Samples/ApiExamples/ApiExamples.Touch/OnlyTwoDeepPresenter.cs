using Cirrious.MvvmCross.Touch.Views.Presenters;
using MonoTouch.UIKit;

namespace ApiExamples.Touch
{
    public class OnlyTwoDeepPresenter : MvxTouchViewPresenter
    {
        public OnlyTwoDeepPresenter(UIApplicationDelegate applicationDelegate, UIWindow window) 
            : base(applicationDelegate, window)
        {
        }

        public override void Show(Cirrious.MvvmCross.Touch.Views.IMvxTouchView view)
        {
            if (MasterNavigationController == null)
            {
                base.Show(view);
                return;
            }

            if (MasterNavigationController.ViewControllers.Length <= 1)
            {
                base.Show(view);
                return;
            }

            MasterNavigationController.PopViewControllerAnimated(false);
            MasterNavigationController.PushViewController(
                view as UIViewController,
                true);
        }
    }
}