using System;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;
using Playground.Core.ViewModels;

namespace Playground.iOS.Views
{
    [MvxFromStoryboard("Main")]
    [MvxDetailSplitViewPresentation]
    public partial class SplitDetailView : MvxViewController<SplitDetailViewModel>
    {
        public SplitDetailView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            btnClose.TouchUpInside += (sender, e) =>
            {
                this.DismissViewController(true, null);
            };
        }
    }
}