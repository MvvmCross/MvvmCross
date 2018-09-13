using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using Playground.Core.ViewModels;

namespace Playground.iOS.Views
{
    [MvxFromStoryboard("Main")]
    [MvxTabPresentation(WrapInNavigationController = true, TabIconName = "home", TabName = "Tab 1")]
    public partial class Tab1View : MvxViewController<Tab1ViewModel>
    {
        public Tab1View(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<Tab1View, Tab1ViewModel>();

            set.Bind(btnModal).To(vm => vm.OpenModalCommand);
            set.Bind(btnNavModal).To(vm => vm.OpenNavModalCommand);
            set.Bind(btnChild).To(vm => vm.OpenChildCommand);
            set.Bind(btnTab2).To(vm => vm.OpenTab2Command);

            set.Apply();
        }
    }
}
