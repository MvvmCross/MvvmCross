using System;
using Eventhooks.Core.ViewModels;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using UIKit;
using MvvmCross.iOS.Views.Presenters.Attributes;

namespace Eventhooks.iOS
{
    [MvxRootPresentation(WrapInNavigationController = true)]
    public partial class FirstView : MvxViewController<FirstViewModel>
    {
        public FirstView() : base("FirstView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            var bindingSet = this.CreateBindingSet<FirstView, FirstViewModel>();
            bindingSet.Bind(SecondViewButton).To(vm => vm.ShowSecondView);
            bindingSet.Apply();

        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

