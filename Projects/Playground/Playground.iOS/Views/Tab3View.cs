using System;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using ObjCRuntime;
using Playground.Core.ViewModels;

namespace Playground.iOS.Views
{
    [MvxFromStoryboard("Main")]
    [MvxTabPresentation(WrapInNavigationController = false)]
    public partial class Tab3View : MvxViewController<Tab3ViewModel>, IMvxTabBarItemViewController
    {
        public Tab3View(NativeHandle handle) : base(handle)
        {
        }

        public string TabName => "Third";
        public string TabIconName => "settings";

        public string TabSelectedIconName => "settings";

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = CreateBindingSet();
            set.Bind(btnShowStack).To(vm => vm.ShowRootViewModelCommand);
            set.Bind(btnClose).To(vm => vm.CloseViewModelCommand);
            set.Apply();
        }
    }
}
