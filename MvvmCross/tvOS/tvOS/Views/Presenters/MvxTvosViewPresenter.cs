// MvxTvosViewPresenter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;
using UIKit;

namespace MvvmCross.tvOS.Views.Presenters
{
    public class MvxTvosViewPresenter
        : MvxBaseTvosViewPresenter
    {
        private readonly IUIApplicationDelegate _applicationDelegate;
        private readonly UIWindow _window;

        public virtual UINavigationController MasterNavigationController
        {
            get; protected set;
        }

        protected virtual IUIApplicationDelegate ApplicationDelegate => _applicationDelegate;

        protected virtual UIWindow Window => _window;

        public MvxTvosViewPresenter(IUIApplicationDelegate applicationDelegate, UIWindow window)
        {
            _applicationDelegate = applicationDelegate;
            _window = window;
        }

        public override void Show(MvxViewModelRequest request)
        {
            var view = this.CreateViewControllerFor(request);

#warning Need to reinsert ClearTop type functionality here
            //if (request.ClearTop)
            //    ClearBackStack();

            Show(view);
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            base.ChangePresentation(hint);

            if (hint is MvxClosePresentationHint)
            {
                Close((hint as MvxClosePresentationHint).ViewModelToClose);
                return;
            }
        }

        public virtual void Show(IMvxTvosView view)
        {
            var viewController = view as UIViewController;
            if (viewController == null)
                throw new MvxException("Passed in IMvxTvosView is not a UIViewController");

            if (MasterNavigationController == null)
                ShowFirstView(viewController);
            else
                MasterNavigationController.PushViewController(viewController, true /*animated*/);
        }

        public virtual void CloseModalViewController()
        {
            MasterNavigationController.PopViewController(true);
        }

        public override void Close(IMvxViewModel toClose)
        {
            var topViewController = MasterNavigationController.TopViewController;

            if (topViewController == null)
            {
                MvxTrace.Warning("Don't know how to close this viewmodel - no topmost");
                return;
            }

            var topView = topViewController as IMvxTvosView;
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

            MasterNavigationController.PopViewController(true);
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

            MasterNavigationController = CreateNavigationController(viewController);

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

        protected virtual UINavigationController CreateNavigationController(UIViewController viewController)
        {
            return new UINavigationController(viewController);
        }

        protected virtual UIViewController CurrentTopViewController => MasterNavigationController.TopViewController;
    }
}