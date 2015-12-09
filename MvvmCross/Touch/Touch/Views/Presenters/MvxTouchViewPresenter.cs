// MvxTouchViewPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Touch.Views.Presenters
{
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;
    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.Platform;

    using UIKit;

    public class MvxTouchViewPresenter
        : MvxBaseTouchViewPresenter
    {
        private readonly IUIApplicationDelegate _applicationDelegate;
        private readonly UIWindow _window;

        public virtual UINavigationController MasterNavigationController
        {
            get; protected set;
        }

        protected virtual IUIApplicationDelegate ApplicationDelegate => this._applicationDelegate;

        protected virtual UIWindow Window => this._window;

        public MvxTouchViewPresenter(IUIApplicationDelegate applicationDelegate, UIWindow window)
        {
            this._applicationDelegate = applicationDelegate;
            this._window = window;
        }

        public override void Show(MvxViewModelRequest request)
        {
            var view = this.CreateViewControllerFor(request);

#warning Need to reinsert ClearTop type functionality here
            //if (request.ClearTop)
            //    ClearBackStack();

            this.Show(view);
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            base.ChangePresentation(hint);

            if (hint is MvxClosePresentationHint)
            {
                this.Close((hint as MvxClosePresentationHint).ViewModelToClose);
                return;
            }
        }

        public virtual void Show(IMvxTouchView view)
        {
            var viewController = view as UIViewController;
            if (viewController == null)
                throw new MvxException("Passed in IMvxTouchView is not a UIViewController");

            if (this.MasterNavigationController == null)
                this.ShowFirstView(viewController);
            else
                this.MasterNavigationController.PushViewController(viewController, true /*animated*/);
        }

        public virtual void CloseModalViewController()
        {
            this.MasterNavigationController.PopViewController(true);
        }

        public virtual void Close(IMvxViewModel toClose)
        {
            var topViewController = this.MasterNavigationController.TopViewController;

            if (topViewController == null)
            {
                MvxTrace.Warning("Don't know how to close this viewmodel - no topmost");
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

            this.MasterNavigationController.PopViewController(true);
        }

        public override bool PresentModalViewController(UIViewController viewController, bool animated)
        {
            this.CurrentTopViewController.PresentViewController(viewController, animated, () => { });
            return true;
        }

        public override void NativeModalViewControllerDisappearedOnItsOwn()
        {
            // ignored
        }

        protected virtual void ShowFirstView(UIViewController viewController)
        {
            foreach (var view in this._window.Subviews)
                view.RemoveFromSuperview();

            this.MasterNavigationController = this.CreateNavigationController(viewController);

            this.OnMasterNavigationControllerCreated();

            this.SetWindowRootViewController(this.MasterNavigationController);
        }

        protected virtual void SetWindowRootViewController(UIViewController controller)
        {
            this._window.AddSubview(controller.View);
            this._window.RootViewController = controller;
        }

        protected virtual void OnMasterNavigationControllerCreated()
        {
        }

        protected virtual UINavigationController CreateNavigationController(UIViewController viewController)
        {
            return new UINavigationController(viewController);
        }

        protected virtual UIViewController CurrentTopViewController => this.MasterNavigationController.TopViewController;
    }
}