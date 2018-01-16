using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;
using Playground.Core.ViewModels;

namespace Playground.iOS.Views
{
    [MvxFromStoryboard("Main")]
    [MvxTabPresentation(WrapInNavigationController = false)]
    public partial class Tab3View : MvxViewController<Tab3ViewModel>, IMvxTabBarItemViewController
    {
        public Tab3View(IntPtr handle) : base(handle)
        {
        }

        public string TabName => "Third";
        public string TabIconName => "settings";

        public string TabSelectedIconName => "settings";

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<Tab3View, Tab3ViewModel>();

            set.Bind(btnShowStack).To(vm => vm.ShowRootViewModelCommand);
            set.Bind(btnClose).To(vm => vm.CloseViewModelCommand);

            set.Apply();
        }
    }
}