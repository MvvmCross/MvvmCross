using Foundation;
using System;
using UIKit;
using MvvmCross.iOS.Views;
using Playground.Core.ViewModels;
using MvvmCross.iOS.Views.Presenters.Attributes;

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