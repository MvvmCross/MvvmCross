using System;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using MonoTouch.UIKit;
using TwitterSearch.UI.Touch.Views;

namespace TwitterSearch.UI.Touch
{
    public class TwitterTabletSearchPresenter
        : MvxBaseTouchViewPresenter
    {
        private readonly UIApplicationDelegate _applicationDelegate;
        private readonly UIWindow _window;
        private readonly SplitViewController _splitView;

        public TwitterTabletSearchPresenter(UIApplicationDelegate applicationDelegate, UIWindow window)
        {
            _applicationDelegate = applicationDelegate;
            _window = window;
            _splitView = new SplitViewController();
            _window.RootViewController = _splitView;
        }

        public override bool ShowView(IMvxTouchView view)
        {
            if (view is HomeView)
            {
                _splitView.SetPrimaryView(view);
            }
            else
            {
                _splitView.SetSecondaryView(view);
            }
			return true;
        }

        public override void NativeModalViewControllerDisappearedOnItsOwn()
        {
            throw new NotImplementedException();
        }

        public override void ClearBackStack()
        {
            throw new NotImplementedException();
        }

        public override bool GoBack()
        {
            throw new NotImplementedException();
        }

        public override bool PresentNativeModalViewController(UIViewController viewController, bool animated)
        {
            throw new NotImplementedException();
        }
    }
}