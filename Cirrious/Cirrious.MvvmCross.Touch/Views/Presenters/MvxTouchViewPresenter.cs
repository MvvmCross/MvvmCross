// MvxTouchViewPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Touch.Views.Presenters
{
    public class MvxTouchViewPresenter
        : MvxBaseTouchViewPresenter
    {
        private readonly UIApplicationDelegate _applicationDelegate;
        private readonly UIWindow _window;

        private UINavigationController _masterNavigationController;

        public virtual UINavigationController MasterNavigationController
        {
            get { return _masterNavigationController; }
        }

        protected virtual UIWindow Window
        {
            get { return _window; }
        }

        public MvxTouchViewPresenter(UIApplicationDelegate applicationDelegate, UIWindow window)
        {
            _applicationDelegate = applicationDelegate;
            _window = window;
        }

        public override void Show(MvxViewModelRequest request)
        {
            var view = CreateView(request);

#warning Need to reinsert ClearTop type functionality here
            //if (request.ClearTop)
            //    ClearBackStack();

            Show(view);
        }

        private IMvxTouchView CreateView(MvxViewModelRequest request)
        {
            return Mvx.Resolve<IMvxTouchViewCreator>().CreateView(request);
        }

        public virtual void Show(IMvxTouchView view)
        {
            var viewController = view as UIViewController;
            if (viewController == null)
                throw new MvxException("Passed in IMvxTouchView is not a UIViewController");

            if (_masterNavigationController == null)
                ShowFirstView(viewController);
            else
                _masterNavigationController.PushViewController(viewController, true /*animated*/);
        }

#warning Unused
		public virtual void CloseModalViewController()
        {
            _masterNavigationController.PopViewControllerAnimated(true);
        }

#warning Unused
		public virtual void Close(IMvxViewModel toClose)
        {
            var topViewController = _masterNavigationController.TopViewController;

            if (topViewController == null)
            {
                MvxTrace.Warning( "Don't know how to close this viewmodel - no topmost");
                return;
            }

            var topView = topViewController as IMvxTouchView;
            if (topView == null)
            {
                MvxTrace.Warning(
                               "Don't know how to close this viewmodel - topmost is not a touchview");
                return;
            }

            var viewModel = topView.ReflectionGetViewModel();
            if (viewModel != toClose)
            {
                MvxTrace.Warning(
                               "Don't know how to close this viewmodel - topmost view does not present this viewmodel");
                return;
            }

            _masterNavigationController.PopViewControllerAnimated(true);
        }

#warning Unused
		public virtual void ClearBackStack()
        {
            if (_masterNavigationController == null)
                return;

            _masterNavigationController.PopToRootViewController(true);
            _masterNavigationController = null;
        }

        public override bool PresentModalViewController(UIViewController viewController, bool animated)
        {
            CurrentTopViewController.PresentViewController(viewController, animated, () => { });
            return true;
        }

        public override void NativeModalViewControllerDisappearedOnItsOwn()
        {
            // ignored
        }

        protected virtual void ShowFirstView(UIViewController viewController)
        {
            foreach (var view in _window.Subviews)
                view.RemoveFromSuperview();

            _masterNavigationController = CreateNavigationController(viewController);

            OnMasterNavigationControllerCreated();

            SetWindowRootViewController(_masterNavigationController);
        }

        protected virtual void SetWindowRootViewController(UIViewController controller)
        {
            _window.AddSubview(controller.View);
            _window.RootViewController = controller;
        }

        protected virtual void OnMasterNavigationControllerCreated()
        {
        }

        protected virtual UINavigationController CreateNavigationController(UIViewController viewController)
        {
            return new UINavigationController(viewController);
        }

        protected virtual UIViewController CurrentTopViewController
        {
            get { return _masterNavigationController.TopViewController; }
        }
    }
}