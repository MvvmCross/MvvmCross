using Cirrious.MvvmCross.Touch.Interfaces;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Touch.Views.Presenters
{
    public class MvxBaseTouchViewPresenter : IMvxTouchViewPresenter
    {
        public virtual bool ShowView(IMvxTouchView view)
        {
            return false;
        }

        public virtual bool GoBack()
        {
            return false;
        }

        public virtual void ClearBackStack()
        {
        }

        public virtual bool PresentNativeModalViewController(UIViewController viewController, bool animated)
        {
            return false;
        }
    }
}