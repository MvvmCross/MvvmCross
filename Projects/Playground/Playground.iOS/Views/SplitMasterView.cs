using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;
using Playground.Core.ViewModels;

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
            set.Bind(btnDetailNav).To(vm => vm.OpenDetailNavCommand);
            set.Bind(btnStack).To(vm => vm.ShowRootViewModel);
            set.Apply();
        }
    }
}