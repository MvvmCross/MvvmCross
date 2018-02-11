﻿using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platform.Ios.Views;
using UIKit;
using Playground.Core.ViewModels;
using MvvmCross.Platform.Ios.Presenters.Attributes;

namespace Playground.iOS.Views
{
    [MvxFromStoryboard("Main")]
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class NestedModalView : MvxViewController<NestedModalViewModel>
    {
        public NestedModalView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.Orange;

            var set = this.CreateBindingSet<NestedModalView, NestedModalViewModel>();

            set.Bind(btnTabs).To(vm => vm.ShowTabsCommand);
            set.Bind(btnClose).To(vm => vm.CloseCommand);

            set.Apply();
        }
    }
}
