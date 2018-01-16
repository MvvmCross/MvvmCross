using System;
using AppKit;
using CoreGraphics;
using MvvmCross.Mac.Views;
using MvvmCross.Mac.Views.Presenters.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Mac
{
    [MvxTabPresentation(TabTitle = "Tab2")]
    public class Tab2View : MvxViewController<Tab2ViewModel>
    {
        public Tab2View() : base()
        {
        }

        public override void LoadView()
        {
            View = new AppKit.NSView(new CGRect(100, 100, 300, 300));
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var lbl = new NSTextField(new CGRect(300, 300, 100, 30)) { Editable = false, Bezeled = false };
            lbl.StringValue = "Tab 2";

            View.AddSubview(lbl);
        }
    }
}
