using System;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;
using Playground.Core.ViewModels;

namespace Playground.iOS.Views
{
    [MvxFromStoryboard("Main")]
    [MvxRootPresentation]
    public partial class TabsRootView : MvxTabBarViewController<TabsRootViewModel>
    {
        private bool _isPresentedFirstTime = true;

        public TabsRootView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if(ViewModel != null && _isPresentedFirstTime)
            {
                _isPresentedFirstTime = false;
                ViewModel.ShowInitialViewModelsCommand.Execute(null);
            }
        }

        protected override void SetTitleAndTabBarItem(UIKit.UIViewController viewController, string title, string iconName)
        {
            // you can override this method to set title or iconName
            if(string.IsNullOrEmpty(title))
                title = "Tab 2";
            if(string.IsNullOrEmpty(iconName))
                iconName = "ic_tabbar_menu";

            base.SetTitleAndTabBarItem(viewController, title, iconName);
        }
    }
}