using System;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters;
using MvvmCross.Platforms.Ios.Views;
using Playground.Core.ViewModels;
using UIKit;

namespace Playground.iOS.Views
{
    [MvxFromStoryboard("Main")]
    public partial class SecondChildView : MvxViewController<SecondChildViewModel>
    {
        public SecondChildView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.Green;

            var set = this.CreateBindingSet<SecondChildView, SecondChildViewModel>();

            set.Bind(btnClose).To(vm => vm.CloseCommand);

            set.Apply();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            btnCloseStack.TouchUpInside += BtnCloseStack_TouchUpInside;
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            btnCloseStack.TouchUpInside -= BtnCloseStack_TouchUpInside;
        }

        private void BtnCloseStack_TouchUpInside(object sender, EventArgs e)
        {
            var appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate;
            var presenter = Mvx.IoCProvider.GetSingleton<IMvxIosViewPresenter>() as MvxIosViewPresenter;

            if (appDelegate.Window.RootViewController.PresentedViewController != null)
            {
                appDelegate.Window.RootViewController.DismissViewController(true, null);
                presenter.CloseModalViewControllers();
            }
            else
            {
                presenter.MasterNavigationController.PopToRootViewController(true);
            }
        }
    }
}
