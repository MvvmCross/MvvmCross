using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;
using RoutingExample.Core.ViewModels;

namespace RoutingExample.iOS
{
    [MvxFromStoryboard("Main")]
    [MvxChildPresentation]
    public partial class TestBViewController : MvxViewController<TestBViewModel>
    {
        public TestBViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var set = this.CreateBindingSet<TestBViewController, TestBViewModel>();
            set.Bind(CloseButton).To(vm => vm.CloseViewModelCommand);
            set.Apply();
        }
    }
}