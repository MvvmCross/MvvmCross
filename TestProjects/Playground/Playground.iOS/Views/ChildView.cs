using Foundation;
using System;
using UIKit;
using Playground.Core.ViewModels;
using MvvmCross.iOS.Views;
using MvvmCross.Binding.BindingContext;

namespace Playground.iOS.Views
{
    [MvxFromStoryboard("Main")]
    public partial class ChildView : MvxViewController<ChildViewModel>
    {
        public ChildView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.Yellow;

            var set = this.CreateBindingSet<ChildView, ChildViewModel>();

            set.Bind(btnClose).To(vm => vm.CloseCommand);
            set.Bind(btnShowSecondChild).To(vm => vm.ShowSecondChildCommand);

            set.Apply();
        }
    }
}