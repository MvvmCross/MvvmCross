using System;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using Playground.Core.ViewModels;

namespace Playground.iOS.Views
{
    [MvxFromStoryboard("Main")]
    [MvxSplitViewPresentation(MasterDetailPosition.Master)]
    public partial class SplitMasterView : MvxViewController<SplitMasterViewModel>
    {
        public SplitMasterView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = CreateBindingSet();
            set.Bind(btnDetail).To(vm => vm.OpenDetailCommand);
            set.Bind(btnDetailNav).To(vm => vm.OpenDetailNavCommand);
            set.Bind(btnStack).To(vm => vm.ShowRootViewModel);
            set.Apply();
        }
    }
}
