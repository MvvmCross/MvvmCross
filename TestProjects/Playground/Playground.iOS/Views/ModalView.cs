using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using Playground.Core.ViewModels;
using UIKit;

namespace Playground.iOS.Views
{
    [MvxModalDisplayStyle(UIModalPresentationStyle.OverFullScreen, UIModalTransitionStyle.CrossDissolve, animated: true)]
    [MvxFromStoryboard("Main")]
    public partial class ModalView : MvxViewController<ModalViewModel>, IMvxModalIosView
    {
        public ModalView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.Orange;

            var set = this.CreateBindingSet<ModalView, ModalViewModel>();

            set.Bind(btnClose).To(vm => vm.CloseCommand);

            set.Apply();
        }
    }
}