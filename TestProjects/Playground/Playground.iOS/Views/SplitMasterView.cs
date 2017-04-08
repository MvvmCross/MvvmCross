using Foundation;
using System;
using UIKit;
using MvvmCross.iOS.Views;
using Playground.Core.ViewModels;
using MvvmCross.iOS.Views.Presenters.Attributes;
using MvvmCross.Binding.BindingContext;

namespace Playground.iOS.Views
{
    [MvxFromStoryboard("Main")]
    [MvxMasterSplitViewPresentation]
    public partial class SplitMasterView : MvxViewController<SplitMasterViewModel>
    {
        public SplitMasterView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<SplitMasterView, SplitMasterViewModel>();
            set.Bind(btnDetail).To(vm => vm.OpenDetailCommand);
            set.Apply();
        }
    }
}