using System;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using ObjCRuntime;
using Playground.Core.ViewModels;
using UIKit;

namespace Playground.iOS.Views
{
    [MvxFromStoryboard("Main")]
    [MvxRootPresentation(WrapInNavigationController = true)]
    public partial class RootView : MvxViewController<RootViewModel>
    {
        public RootView(NativeHandle handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.LightGray;

            using (var set = CreateBindingSet())
            {
                set.Bind(btnTabs).To(vm => vm.ShowTabsCommand);
                set.Bind(btnPages).To(vm => vm.ShowPagesCommand);
                set.Bind(btnSplit).To(vm => vm.ShowSplitCommand);
                set.Bind(btnChild).To(vm => vm.ShowChildCommand);
                set.Bind(btnModal).To(vm => vm.ShowModalCommand);
                set.Bind(btnNavModal).To(vm => vm.ShowModalNavCommand);
                set.Bind(btnOverrideAttribute).To(vm => vm.ShowOverrideAttributeCommand);
                set.Bind(btnShowCustomBinding).To(vm => vm.ShowCustomBindingCommand);
            }
        }
    }
}
