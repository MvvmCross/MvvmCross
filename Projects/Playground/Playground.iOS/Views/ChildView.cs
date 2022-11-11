using System;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using Playground.Core.ViewModels;
using UIKit;

namespace Playground.iOS.Views
{
    [MvxFromStoryboard("Main")]
    [MvxChildPresentation]
    public partial class ChildView : MvxViewController<ChildViewModel>
    {
        public ChildView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.Yellow;

            var set = CreateBindingSet();
            set.Bind(btnClose).To(vm => vm.CloseCommand);
            set.Bind(btnShowSecondChild).To(vm => vm.ShowSecondChildCommand);
            set.Apply();
        }
    }
}
