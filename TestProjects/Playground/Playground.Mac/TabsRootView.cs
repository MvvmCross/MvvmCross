using System;
using MvvmCross.Mac.Views;
using MvvmCross.Mac.Views.Presenters.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Mac
{
    [MvxWindowPresentation(PositionX = 150)]
    public class TabsRootView : MvxTabViewController<TabsRootViewModel>
    {
        private bool _firstTime = true;

        public TabsRootView()
        {
        }

        public override void LoadView()
        {
            base.LoadView();

            View = View ?? new AppKit.NSView();
        }

        public override void ViewWillAppear()
        {
            base.ViewWillAppear();

            if (_firstTime)
            {
                ViewModel.ShowInitialViewModelsCommand.Execute(null);
                _firstTime = false;
            }
        }
    }
}
