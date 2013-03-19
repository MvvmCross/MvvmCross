using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.Dialog.Touch;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using MonoTouch.UIKit;
using Tutorial.Core.ViewModels.Lessons;

namespace Tutorial.UI.Touch.Views.Lessons
{
    public sealed class CompositeView
         : MvxTabBarViewController
    {
        bool _viewDidLoadCallNeeded;
        public CompositeView() 
        {
			if (_viewDidLoadCallNeeded)
                ViewDidLoad();
        }

		public new CompositeViewModel ViewModel {
			get { return (CompositeViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (ViewModel == null)
            {
                _viewDidLoadCallNeeded = true;
                return;
            }

            ViewControllers = new UIViewController[]
                                  {
                                    CreateTabFor("Tip", "speakers", ViewModel.Tip),
                                    CreateTabFor("Text", "speakers", ViewModel.Text),
                                    CreateTabFor("Pull", "speakers", ViewModel.Pull),
                                  };

            // tell the tab bar which controllers are allowed to customize. 
            // if we don't set  it assumes all controllers are customizable. 
            // if we set to empty array, NO controllers are customizable.
            CustomizableViewControllers = new UIViewController[] { };

            // set our selected item
            SelectedViewController = ViewControllers[0];
        }

        private int _createdSoFarCount = 0;
        private UIViewController CreateTabFor(string title, string imageName, IMvxViewModel viewModel)
        {
            var innerView = (UIViewController)this.CreateViewControllerFor(viewModel);
            innerView.Title = title;
            innerView.TabBarItem = new UITabBarItem(
                                    title, 
                                    UIImage.FromBundle("Images/Tabs/" + imageName + ".png"),
                                    _createdSoFarCount++);
            return innerView;
        }
    }
}
