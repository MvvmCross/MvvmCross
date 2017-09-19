// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using AppKit;
using MvvmCross.Mac.Views;
using MvvmCross.Mac.Views.Presenters.Attributes;
using MvvmCross.Binding.BindingContext;
using Playground.Core.ViewModels;

namespace Playground.Mac
{
    [MvxFromStoryboard("Main")]
    [MvxWindowPresentation("ToolbarWindow", "Main", Width = 500)]
    public partial class WindowView : MvxViewController<WindowViewModel>
    {
        public WindowView(IntPtr handle) : base(handle)
        {
            Title = "Window view";
        }

        public ToolbarWindow WindowController {
            get { return View.Window != null ? (ToolbarWindow)View.Window.WindowController : null; }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<WindowView, WindowViewModel>();
            set.Bind(btnClose).To(vm => vm.CloseCommand);
            set.Apply();
        }

        public override void ViewDidAppear()
        {
            base.ViewDidAppear();

            //WindowController.MenuItemSetting.State = ViewModel.IsItemSetting ? NSCellStateValue.On : NSCellStateValue.Off;

            var set = this.CreateBindingSet<WindowView, WindowViewModel>();
            set.Bind(WindowController.TextTitle).For(v => v.StringValue).To(vm => vm.Title);
            set.Bind(WindowController.MenuItem1).For(v => v.State).To(vm => vm.IsItem1);
            set.Bind(WindowController.MenuItem2).For(v => v.State).To(vm => vm.IsItem2);
            set.Bind(WindowController.MenuItem3).For(v => v.State).To(vm => vm.IsItem3);
            set.Bind(WindowController.MenuItemSetting).To(vm => vm.ToggleSettingCommand);
            set.Bind(WindowController.MenuItemSetting).For(v => v.State).To(vm => vm.IsItemSetting).OneWay();
            set.Apply();

            GC.Collect();       // test to make sure WindowController does not get removed prematurely
        }
    }
}
