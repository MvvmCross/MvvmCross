﻿using System;
using MvvmCross.Platform.Ios.Presenters.Attributes;
using MvvmCross.Platform.Ios.Views;
using MvvmCross.ViewModels;
using Playground.Core.ViewModels;
using UIKit;

namespace Playground.iOS.Views
{
    [MvxFromStoryboard("Main")]
    [MvxRootPresentation(WrapInNavigationController = true)]
    public partial class TabsRootView : MvxTabBarViewController<TabsRootViewModel>
    {
        private bool _isPresentedFirstTime = true;

        public TabsRootView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (ViewModel != null && _isPresentedFirstTime)
            {
                _isPresentedFirstTime = false;
                ViewModel.ShowInitialViewModelsCommand.ExecuteAsync(null);
            }
        }

        protected override void SetTitleAndTabBarItem(UIViewController viewController, MvxTabPresentationAttribute attribute)
        {
            // you can override this method to set title or iconName
            if (string.IsNullOrEmpty(attribute.TabName))
                attribute.TabName = "Tab 2";
            if (string.IsNullOrEmpty(attribute.TabIconName))
                attribute.TabIconName = "ic_tabbar_menu";

            base.SetTitleAndTabBarItem(viewController, attribute);
        }

        public override bool ShowChildView(UIViewController viewController)
        {
            var type = viewController.GetType();

            return type == typeof(ChildView)
                ? false
                : base.ShowChildView(viewController);
        }

        public override bool CloseChildViewModel(IMvxViewModel viewModel)
        {
            var type = viewModel.GetType();

            return type == typeof(ChildViewModel)
                ? false
                : base.CloseChildViewModel(viewModel);
        }
    }
}
