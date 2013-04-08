using System;
using System.Collections.Generic;
using Cirrious.Conference.Core.ViewModels;
using Cirrious.Conference.Core.ViewModels.HomeViewModels;
using Cirrious.Conference.Core.ViewModels.SessionLists;
using Cirrious.Conference.UI.Touch.Interfaces;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Binding.Touch.Views;

namespace Cirrious.Conference.UI.Touch.Views
{
    public class TabBarController
        : MvxTabBarViewController
        , ITabBarPresenter        
    {
		public TabBarController()
        {
            Mvx.Resolve<ITabBarPresenterHost>().TabBarPresenter = this;

			// because the UIKit base class does ViewDidLoad, we have to make a second call here
			ViewDidLoad();
		}

		public new HomeViewModel ViewModel {
			get { return (HomeViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

		private int _createdSoFarCount = 0;

        private UIViewController CreateTabFor(string title, string imageName, IMvxViewModel viewModel)
        {
            var controller = new UINavigationController();
            controller.NavigationBar.TintColor = UIColor.Black;
            var screen = this.CreateViewControllerFor(viewModel) as UIViewController;
            SetTitleAndTabBarItem(screen, title, imageName);
            controller.PushViewController(screen, false);
            return controller;
        }

        private void SetTitleAndTabBarItem(UIViewController screen, string title, string imageName)
        {
            screen.Title = ViewModel.TextSource.GetText(title);
            screen.TabBarItem = new UITabBarItem(title, UIImage.FromBundle("Images/Tabs/" + imageName + ".png"),
                                                 _createdSoFarCount);
            _createdSoFarCount++;
        }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
		
			// first time around this will be null, second time it will be OK
			if (ViewModel == null)
				return;

		    var viewControllers = new UIViewController[]
                                  {
                                    CreateTabFor("Welcome", "home", ViewModel.Welcome),
                                    CreateTabFor("Sessions", "sessions", ViewModel.Sessions),
                                    CreateTabFor("Favorites", "favorites", ViewModel.Favorites),
                                    CreateTabFor("Tweets", "twitter", ViewModel.Twitter),
                                  };
            ViewControllers = viewControllers;
            CustomizableViewControllers = new UIViewController[] { };
            SelectedViewController = ViewControllers[0];
        }

        /// <summary>
        /// Only allow iPad application to rotate, iPhone is always portrait
        /// </summary>
        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
        {
            if (AppDelegate.IsPad)
                return true;
            else
                return toInterfaceOrientation == UIInterfaceOrientation.Portrait;
        }

        public bool GoBack()
        {
            var subNavigation = this.SelectedViewController as UINavigationController;
            if (subNavigation == null)
                return false;

            if (subNavigation.ViewControllers.Length <= 1)
                return false;

            subNavigation.PopViewControllerAnimated(true);
            return true;
        }

        public bool ShowView(IMvxTouchView view)
        {
            if (TryShowViewInCurrentTab(view))
                return true;

            return false;
        }

        private bool TryShowViewInCurrentTab(IMvxTouchView view)
        {
            var navigationController = (UINavigationController)this.SelectedViewController;
            navigationController.PushViewController((UIViewController)view, true);
            return true;
        }
    }
}