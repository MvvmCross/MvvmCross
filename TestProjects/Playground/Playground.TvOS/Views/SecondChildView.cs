using System;

using MvvmCross.Binding.BindingContext;
using MvvmCross.tvOS.Views;
using MvvmCross.tvOS.Views.Presenters;
using MvvmCross.Platform;
using MvvmCross.Platform.tvOS.Views;
using MvvmCross.tvOS.Views.Presenters.Attributes;

using Playground.Core.ViewModels;

using Foundation;
using UIKit;

namespace Playground.TvOS
{
    [MvxFromStoryboard("Main")]
    [MvxChildPresentation]
    public partial class SecondChildView : MvxViewController<SecondChildViewModel>
	{
		public SecondChildView (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<SecondChildView, SecondChildViewModel>();
            set.Bind(btnClose).To(vm => vm.CloseCommand);
            set.Apply();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            btnCloseStack.PrimaryActionTriggered += BtnCloseStack_OnPrimaryATcionTriggered;
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            btnCloseStack.PrimaryActionTriggered -= BtnCloseStack_OnPrimaryATcionTriggered;
        }

        private void BtnCloseStack_OnPrimaryATcionTriggered(object sender, EventArgs e)
        {
            var appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate;
            var presenter = Mvx.GetSingleton<IMvxTvosModalHost>() as MvxTvosViewPresenter;

            if (appDelegate.Window.RootViewController.PresentedViewController != null)
            {
                appDelegate.Window.RootViewController.DismissViewController(true, null);
                presenter.NativeModalViewControllerDisappearedOnItsOwn();
            }
            else
            {
                presenter.MasterNavigationController.PopToRootViewController(true);
            }
        }
    }
}
