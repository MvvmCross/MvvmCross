﻿using System;
using MvvmCross.Platforms.Tvos.Presenters.Attributes;
using MvvmCross.Platforms.Tvos.Views;
using Playground.Core.ViewModels;

namespace Playground.TvOS
{
    [MvxFromStoryboard("Main")]
    [MvxRootPresentation(WrapInNavigationController = true)]
    public partial class RootView : MvxViewController<RootViewModel>
	{
		public RootView (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = CreateBindingSet();
            set.Bind(btnChild).To(vm => vm.ShowChildCommand);
            set.Bind(btnTabs).To(vm => vm.ShowTabsCommand);
            set.Bind(btnModalNav).To(vm => vm.ShowModalNavCommand);
            set.Bind(btnModal).To(vm => vm.ShowModalCommand);
            set.Bind(btnModalAttribute).To(vm => vm.ShowOverrideAttributeCommand);
            set.Bind(btnSplitNav).To(vm => vm.ShowSplitCommand);
            set.Apply();
        }
	}
}
