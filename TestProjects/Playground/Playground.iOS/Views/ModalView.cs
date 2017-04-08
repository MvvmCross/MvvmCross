using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;
using Playground.Core.ViewModels;
using UIKit;

namespace Playground.iOS.Views
{
    [MvxFromStoryboard("Main")]
    [MvxModalPresentation(ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen, ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve)]
    public partial class ModalView : MvxViewController<ModalViewModel>
    {
        public ModalView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.Orange;

            var set = this.CreateBindingSet<ModalView, ModalViewModel>();

            set.Bind(btnTabs).To(vm => vm.ShowTabsCommand);
            set.Bind(btnClose).To(vm => vm.CloseCommand);

            set.Apply();
        }
    }
}