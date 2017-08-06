using System;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;
using Playground.Core.ViewModels;

namespace Playground.iOS.Views
{
    [MvxFromStoryboard("Main")]
    [MvxRootPresentation]
    public partial class SplitRootView : MvxSplitViewController<SplitRootViewModel>
    {
        private bool _isPresentedFirstTime = true;

        public SplitRootView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            PreferredPrimaryColumnWidthFraction = .3f;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (ViewModel != null && _isPresentedFirstTime)
            {
                _isPresentedFirstTime = false;
                ViewModel.ShowInitialMenuCommand.Execute(null);

                //PreferredDisplayMode = UISplitViewControllerDisplayMode.PrimaryHidden;
            }
        }
    }
}