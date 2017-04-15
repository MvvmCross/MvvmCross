using Foundation;
using System;
using UIKit;
using MvvmCross.iOS.Views;
using Playground.Core.ViewModels;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views.Presenters.Attributes;

namespace Playground.iOS.Views
{
    [MvxFromStoryboard("Main")]
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class ModalNavView : MvxViewController<ModalNavViewModel>
    {
        public ModalNavView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.Red;

            var set = this.CreateBindingSet<ModalNavView, ModalNavViewModel>();

            set.Bind(btnShowChild).To(vm => vm.ShowChildCommand);
            set.Bind(btnClose).To(vm => vm.CloseCommand);

            set.Apply();
        }
    }
}