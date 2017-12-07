using System;

using MvvmCross.Binding.BindingContext;
using MvvmCross.tvOS.Views;
using MvvmCross.tvOS.Views.Presenters.Attributes;

using Playground.Core.ViewModels;

namespace Playground.TvOS
{
    [MvxFromStoryboard("Main")]
    [MvxDetailSplitViewPresentation(WrapInNavigationController = true)]
    public partial class SplitDetailNavView : MvxViewController<SplitDetailNavViewModel>
	{
		public SplitDetailNavView (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            btnClose.AllEvents += (sender, e) =>
            {
                DismissViewController(true, null);
            };
        }
	}
}
