#region Copyright
// <copyright file="MvxTouchViewPresenter.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Platform.Diagnostics;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Views;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Touch.Views.Presenters
{
    public class MvxTouchViewPresenter 
        : MvxBaseTouchViewPresenter
        , IMvxServiceConsumer<IMvxTouchViewCreator>
    {
        private readonly UIApplicationDelegate _applicationDelegate;
        private readonly UIWindow _window;
        
        private UINavigationController _masterNavigationController;
        
        public MvxTouchViewPresenter (UIApplicationDelegate applicationDelegate, UIWindow window)
        {
            _applicationDelegate = applicationDelegate;
            _window = window;
        } 

        public override void Show(MvxShowViewModelRequest request)
        {
            var view = CreateView(request);

            if (request.ClearTop)
                ClearBackStack();

            Show(view);
        }

        private IMvxTouchView CreateView(MvxShowViewModelRequest request)
        {
            return this.GetService<IMvxTouchViewCreator>().CreateView(request);
        }

        public virtual void Show (IMvxTouchView view)
        {			
            var viewController = view as UIViewController;
            if (viewController == null)
                throw new MvxException("Passed in IMvxTouchView is not a UIViewController");
            
            if (_masterNavigationController == null)
                ShowFirstView(viewController);
            else
                _masterNavigationController.PushViewController(viewController, true /*animated*/);
        }

        public override void CloseModalViewController()
        {            
            _masterNavigationController.PopViewControllerAnimated(true);
        }

        public override void Close(IMvxViewModel toClose)
        {
            var topViewController = _masterNavigationController.TopViewController;

            if (topViewController == null)
            {
                MvxTrace.Trace(MvxTraceLevel.Warning, "Don't know how to close this viewmodel - no topmost");
                return;
            }

            var topView = topViewController as IMvxTouchView;
            if (topView == null)
            {
                MvxTrace.Trace(MvxTraceLevel.Warning, "Don't know how to close this viewmodel - topmost is not a touchview");
                return;
            }

            var viewModel = topView.ReflectionGetViewModel();
            if (viewModel != toClose)
            {
                MvxTrace.Trace(MvxTraceLevel.Warning, "Don't know how to close this viewmodel - topmost view does not present this viewmodel");
                return;
            }

            _masterNavigationController.PopViewControllerAnimated(true);
        }

        public override void ClearBackStack()
        {
            if (_masterNavigationController == null)
                return;
            
            _masterNavigationController.PopToRootViewController (true);
            _masterNavigationController = null;
        }

        public override bool PresentModalViewController(UIViewController viewController, bool animated)
        {
            CurrentTopViewController.PresentModalViewController(viewController, animated);
            return true;
        }

        public override void NativeModalViewControllerDisappearedOnItsOwn()
        {
            // ignored
        }

        protected virtual void ShowFirstView (UIViewController viewController)
        {
            foreach (var view in _window.Subviews)
                view.RemoveFromSuperview();
            
            _masterNavigationController = CreateNavigationController(viewController);

            OnMasterNavigationControllerCreated();

            _window.AddSubview(_masterNavigationController.View);
        }
        
        protected virtual void OnMasterNavigationControllerCreated ()
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
