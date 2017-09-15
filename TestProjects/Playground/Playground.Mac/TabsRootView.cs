using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Mac.Views;
using MvvmCross.Mac.Views.Presenters.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Mac
{
    [MvxWindowPresentation(PositionX = 150)]
    public partial class TabsRootView : MvxTabViewController<TabsRootViewModel>
    {
        private bool _firstTime = true;

        public TabsRootView(IntPtr handle) : base(handle)
        {
        }

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

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public override void ViewDidAppear()
        {
            base.ViewDidAppear();

            var set = this.CreateBindingSet<TabsRootView, TabsRootViewModel>();
            set.Bind(this).For(v => v.SelectedTabViewItemIndex).To(vm => vm.ItemIndex);
            set.Apply();
        }
    }
}