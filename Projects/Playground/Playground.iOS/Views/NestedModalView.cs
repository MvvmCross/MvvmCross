using System;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using Playground.Core.ViewModels;
using UIKit;

namespace Playground.iOS.Views
{
    [MvxFromStoryboard("Main")]
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class NestedModalView : MvxViewController<NestedModalViewModel>
    {
        public NestedModalView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.Orange;

            var set = CreateBindingSet();
            set.Bind(btnTabs).To(vm => vm.ShowTabsCommand);
            set.Bind(btnClose).To(vm => vm.CloseCommand);
            set.Apply();
        }
    }
}
