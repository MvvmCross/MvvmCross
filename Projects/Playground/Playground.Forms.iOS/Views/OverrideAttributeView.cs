using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform.Ios.Views;
using MvvmCross.Platform.Ios.Views.Presenters.Attributes;
using Playground.Core.ViewModels;

using UIKit;

namespace Playground.iOS.Forms.Views
{
    [MvxFromStoryboard("Main")]
    public partial class OverrideAttributeView : MvxViewController<OverrideAttributeViewModel>, IMvxOverridePresentationAttribute
    {
        public OverrideAttributeView()
        {
        }

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

            set.Bind(btnTabs).To(vm => vm.ShowTabsCommand);
            set.Bind(btnClose).To(vm => vm.CloseCommand);

            set.Apply();
        }
    }
}
