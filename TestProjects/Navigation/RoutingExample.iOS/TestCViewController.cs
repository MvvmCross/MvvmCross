using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;
using RoutingExample.Core.ViewModels;
using UIKit;

namespace RoutingExample.iOS
{
    [MvxFromStoryboard("Main")]
    [MvxChildPresentation]
    public partial class TestCViewController : MvxViewController<TestCViewModel>
    {
        public TestCViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var set = this.CreateBindingSet<TestCViewController, TestCViewModel>();
            set.Bind(CloseButton).To(vm => vm.CloseViewModelCommand);
            set.Bind(ValueLabel).To(vm => vm.UserId);
            set.Apply();
        }
    }
}