using System;
using MvvmCross.Platforms.Tvos.Presenters.Attributes;
using MvvmCross.Platforms.Tvos.Views;
using Playground.Core.ViewModels;

namespace Playground.TvOS
{
    [MvxFromStoryboard("Main")]
    [MvxMasterDetailPresentation(WrapInNavigationController = true, 
                                 Position = MasterDetailPosition.Detail)]
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
