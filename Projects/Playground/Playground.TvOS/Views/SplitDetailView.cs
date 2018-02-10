using System;
using MvvmCross.Platform.Tvos.Views;
using Playground.Core.ViewModels;

namespace Playground.TvOS
{
    [MvxFromStoryboard("Main")]
    [MvxMasterDetailPresentation(Position = MasterDetailPosition.Detail)]
    public partial class SplitDetailView : MvxViewController<SplitDetailViewModel>
	{
		public SplitDetailView (IntPtr handle) : base (handle)
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
