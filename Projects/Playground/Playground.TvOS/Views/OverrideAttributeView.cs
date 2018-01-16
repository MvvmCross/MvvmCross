using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.tvOS.Views;
using MvvmCross.tvOS.Views.Presenters.Attributes;
using Playground.Core.ViewModels;
using UIKit;

namespace Playground.TvOS
{
    [MvxFromStoryboard("Main")]
    public partial class OverrideAttributeView
        : MvxViewController<OverrideAttributeViewModel>, IMvxOverridePresentationAttribute
    {
        public OverrideAttributeView(IntPtr handle) : base(handle)
        {
        }

        public MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request)
        {
            return new MvxModalPresentationAttribute
            {
                ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen,
                ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve
            };
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.Cyan;

            var set = this.CreateBindingSet<OverrideAttributeView, OverrideAttributeViewModel>();

            set.Bind(btnTabNav).To(vm => vm.ShowTabsCommand);
            set.Bind(btnClose).To(vm => vm.CloseCommand);

            set.Apply();
        }

    }
}
