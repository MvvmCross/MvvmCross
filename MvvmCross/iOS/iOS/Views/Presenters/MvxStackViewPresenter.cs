using System;
using System.Linq;
using MvvmCross.Core.ViewModels;
using UIKit;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;

namespace MvvmCross.iOS.Views.Presenters
{
    public class MvxStackViewPresenter : MvxBaseIosViewPresenter
    {
        private readonly IUIApplicationDelegate _applicationDelegate;
        private readonly UIWindow _window;

        public MvxNavigationController MasterNavigationController { get; set; }

        public MvxNavigationController ModalNavigationController { get; set; }

        public MvxStackViewPresenter(IUIApplicationDelegate applicationDelegate, UIWindow window)
        {
            _applicationDelegate = applicationDelegate;
            _window = window;
        }

        protected virtual UIViewController CurrentTopViewController
        {
            get
            {
                // exploring the stack manually instead checking MasterNavigationController / ModalNavigationController
                // allows the user to present his own modals outside the presenter without problem

                var windowRoot = _window.RootViewController;

                // check for a presented ViewController
                if(windowRoot.PresentedViewController != null)
                {
                    var presentedViewController = windowRoot.PresentedViewController;

                    // the presented ViewController can also have a stack of ViewControllers
                    if(presentedViewController.GetType().Equals(typeof(UINavigationController)) || presentedViewController.GetType().IsSubclassOf(typeof(UINavigationController)))
                        return (presentedViewController as UINavigationController).TopViewController;
                    else
                        return windowRoot.PresentedViewController;
                }

                if(windowRoot.GetType().Equals(typeof(UINavigationController)) || windowRoot.GetType().IsSubclassOf(typeof(UINavigationController)))
                    return (windowRoot as UINavigationController).TopViewController;

                return windowRoot;
            }
        }

        public override void Show(MvxViewModelRequest request)
        {
            var view = this.CreateViewControllerFor(request);

#warning Need to reinsert ClearTop type functionality here
            //if (request.ClearTop)
            //    ClearBackStack();

            Show(view);
        }

        public void Show(IMvxIosView view)
        {
            var viewController = view as UIViewController;

            // if there is no master NavigationController, it means the app is initializing
            if(MasterNavigationController == null)
            {
                ShowFirstView(viewController);
                return;
            }

            if(view is IMvxModalIosView)
            {
                // a modal ViewController can request by itself to be wrapped in a NavigationController
                if(view is IMvxNavModalIosView)
                {
                    ModalNavigationController = CreateNavigationController(viewController) as MvxNavigationController;
                    viewController = ModalNavigationController;
                }

                // setup presentation and transition for modal view
                var attribute = GetMvxModalDisplayStyleAttribute(viewController);
                var animated = true;

                if(attribute != null)
                {
                    if(attribute.ModalPresentationStyle.HasValue)
                        viewController.ModalPresentationStyle = attribute.ModalPresentationStyle.Value;

                    if(attribute.ModalTransitionStyle.HasValue)
                        viewController.ModalTransitionStyle = attribute.ModalTransitionStyle.Value;

                    animated = attribute.Animated;
                }

                PresentModalViewController(viewController, animated);
                return;
            }

            // display the view in the current navigation controller
            if(CurrentTopViewController.NavigationController != null)
            {
                CurrentTopViewController.NavigationController.PushViewController(viewController, true);
            }
            else
            {
                throw new MvxException("Trying to show a ViewModel when there is no current visible NavigationController");
            }
        }

        public override bool PresentModalViewController(UIViewController viewController, bool animated)
        {
            // if there is currently a modal ViewController, dismiss it (otherwise nothing happens when presenting)
            if(_window.RootViewController.PresentedViewController != null)
                _window.RootViewController.DismissViewController(animated, null);

            _window.RootViewController.PresentViewController(viewController, animated, null);

            return true;
        }

        public void Close(IMvxViewModel toClose)
        {
            Close(toClose, true);
        }

        public void Close(IMvxViewModel toClose, bool animated)
        {
            // check if toClose is a modal ViewController that is NOT wrapped in a navigation controller
            if(_window.RootViewController.PresentedViewController != null
               && _window.RootViewController.PresentedViewController as MvxNavigationController == null)
            {
                var mvxIosView = _window.RootViewController.PresentedViewController.GetIMvxIosView();
                if(mvxIosView != null)
                {
                    if(mvxIosView.ViewModel == toClose)
                    {
                        _window.RootViewController.DismissViewController(animated, null);
                        return;
                    }
                }
            }

            if(CurrentTopViewController.NavigationController == null)
            {
                MvxTrace.Warning($"Don't know how to close ViewModel of type: {toClose.GetType().Name} - There is no current NavigationController");
                return;
            }

            // check if the current navigation controller is ModalNavigationController
            if(CurrentTopViewController.NavigationController == ModalNavigationController)
            {
                // if the ViewModel to close is the root of the modal navigation stack, then close the entire stack
                CloseModalViewController(toClose, true);
                return;
            }

            if(CurrentTopViewController.NavigationController == MasterNavigationController)
            {
                CloseViewControllerInNavigationController(toClose, MasterNavigationController, animated);
                return;
            }
        }

        private void CloseViewControllerInNavigationController(IMvxViewModel toClose, UINavigationController navController, bool animated)
        {
            // check if toClose is the top most ViewController
            var topViewController = navController.TopViewController.GetIMvxIosView();
            if(topViewController.ViewModel == toClose)
            {
                navController.PopViewController(animated);
                return;
            }

            // loop stack 
            foreach(var viewController in navController.ViewControllers)
            {
                var mvxView = viewController.GetIMvxIosView();
                if(mvxView.ViewModel == toClose)
                {
                    var newViewControllers = navController.ViewControllers.Where(v => v != viewController).ToArray();
                    navController.ViewControllers = newViewControllers;
                    break;
                }
            }
        }

        public void CloseModalViewController(IMvxViewModel toClose, bool animated)
        {
            if(_window.RootViewController.PresentedViewController == null)
                return;

            if(_window.RootViewController.PresentedViewController == ModalNavigationController)
            {
                var modalViewController = ModalNavigationController.ViewControllers.First();

                var mvxIosView = modalViewController.GetIMvxIosView();
                if(mvxIosView.ViewModel == toClose)
                {
                    _window.RootViewController.DismissViewController(animated, null);
                    ModalNavigationController = null;
                }
                else
                {
                    CloseViewControllerInNavigationController(toClose, ModalNavigationController, animated);
                }

                return;
            }

            // if modal presented is not part of any stack, close it anyways
            _window.RootViewController.DismissViewController(animated, null);
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            base.ChangePresentation(hint);

            if(hint is MvxClosePresentationHint)
            {
                Close((hint as MvxClosePresentationHint).ViewModelToClose);
                return;
            }
        }

        public override void NativeModalViewControllerDisappearedOnItsOwn()
        {
            _window.RootViewController.DismissViewController(false, null);
            ModalNavigationController = null;
        }

        protected virtual void ShowFirstView(UIViewController viewController)
        {
            foreach(var view in _window.Subviews)
                view.RemoveFromSuperview();

            MasterNavigationController = CreateNavigationController(viewController) as MvxNavigationController;

            OnMasterNavigationControllerCreated();

            SetWindowRootViewController(MasterNavigationController);
        }

        protected virtual void SetWindowRootViewController(UIViewController controller)
        {
            _window.AddSubview(controller.View);
            _window.RootViewController = controller;
        }

        protected virtual void OnMasterNavigationControllerCreated()
        {
        }

        protected virtual UINavigationController CreateNavigationController(UIViewController rootViewController)
        {
            return new MvxNavigationController(rootViewController);
        }

        private MvxModalDisplayStyleAttribute GetMvxModalDisplayStyleAttribute(UIViewController viewController)
        {
            return viewController.GetType().GetCustomAttributes(typeof(MvxModalDisplayStyleAttribute), true).FirstOrDefault() as MvxModalDisplayStyleAttribute;
        }
    }
}
