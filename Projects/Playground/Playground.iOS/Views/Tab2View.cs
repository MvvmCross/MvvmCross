using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;
using Playground.Core.ViewModels;

namespace Playground.iOS.Views
{
    [MvxFromStoryboard("Main")]
    [MvxTabPresentation]
    public partial class Tab2View : MvxViewController<Tab2ViewModel>
    {
        public Tab2View(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<Tab2View, Tab2ViewModel>();

            set.Bind(btnShowStack).To(vm => vm.ShowRootViewModelCommand);
            set.Bind(btnClose).To(vm => vm.CloseViewModelCommand);

            set.Apply();
        }
    }
}