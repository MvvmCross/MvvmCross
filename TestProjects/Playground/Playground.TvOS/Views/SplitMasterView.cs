using System;

using MvvmCross.Binding.BindingContext;
using MvvmCross.tvOS.Views;
using MvvmCross.tvOS.Views.Presenters.Attributes;

using Playground.Core.ViewModels;

namespace Playground.TvOS
{
    [MvxFromStoryboard("Main")]
    [MvxMasterSplitViewPresentation]
	public partial class SplitMasterView : MvxViewController<SplitMasterViewModel> 
	{
		public SplitMasterView (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var bindingSet = this.CreateBindingSet<SplitMasterView, SplitMasterViewModel>();

            bindingSet.Apply();
        }
	}
}
