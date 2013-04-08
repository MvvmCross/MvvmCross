using System;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;
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

        public override void Show(MvxViewModelRequest request)
        {
            var view = Mvx.Resolve<IMvxTouchViewCreator>().CreateView(request);
            Show(view);
        }

        private void Show(IMvxTouchView view)
        {
            if (view is HomeView)
            {
                _splitView.SetPrimaryView(view);
            }
            else
            {
                _splitView.SetSecondaryView(view);
            }
        }

        public override void NativeModalViewControllerDisappearedOnItsOwn()
        {
            throw new NotImplementedException();
        }

        
        public override bool PresentModalViewController(UIViewController viewController, bool animated)
        {
            throw new NotImplementedException();
        }
    }
}