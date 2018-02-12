﻿using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platform.Tvos.Views;
using UIKit;
using Playground.Core.ViewModels;
using MvvmCross.Platform.Tvos.Presenters.Attributes;

namespace Playground.TvOS
{
    [MvxFromStoryboard("Main")]
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class NestedModalView : MvxViewController<NestedModalViewModel>
	{
		public NestedModalView (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.Blue;

            var set = this.CreateBindingSet<NestedModalView, NestedModalViewModel>();

            set.Bind(btnTabs).To(vm => vm.ShowTabsCommand);
            set.Bind(btnClose).To(vm => vm.CloseCommand);

            set.Apply();
        }
	}
}
