using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform;
using MvvmCross.Platform.iOS.Views;
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
            var presenter = Mvx.GetSingleton<IMvxIosModalHost>() as MvxBaseIosViewPresenter;
            presenter.NativeModalViewControllerDisappearedOnItsOwn();
        }
    }
}