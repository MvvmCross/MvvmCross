using System;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreAnimation;
using System.Linq;

namespace MonoCross.Touch
{
#if false	
	public enum MXTouchAnimation
	{
		Left,
		Right
	}

	
	internal class TouchNavigationHelper
	{
		// when on iPad
		UISplitViewController _splitViewController = null;
		UITabBarController _tabBarController = null;
		SplitViewControllerDelegate _splitViewControllerDelegate = null;
		UINavigationController _detailNavigationController = null;
			
		// detail pane or master when on when on iPod Touch/iPhone
		UINavigationController _masterNavigationController = null;
		
		string _initialViewImage = string.Empty;
		MXTouchNavigationStyle _navStyle;
			
		public TouchNavigationHelper (MXTouchNavigationStyle navStyle, string initialViewImage)
		{
			_navStyle = navStyle;
			_initialViewImage = initialViewImage;
		}
		
		public UIView View { get; set; }

		public bool IsSplit
		{
			get { return _splitViewController != null; }
		}
		public bool HasTabBar { get { return _tabBarController != null; }}
		
		public void Init(UIViewController vc)
		{
			_masterNavigationController = new UINavigationController(vc);

			if (_navStyle == MXTouchNavigationStyle.SplitViewOnPad && UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
			{
				// toss a generic view in the detail pane
				_detailNavigationController = new UINavigationController(new SplashViewController(_initialViewImage));
				
				_splitViewController = new SplitViewController();
				_splitViewControllerDelegate = new SplitViewControllerDelegate();
				_splitViewController.Delegate = _splitViewControllerDelegate;
				_splitViewController.ViewControllers = new UIViewController[2] { _masterNavigationController, _detailNavigationController };

				this.View = _splitViewController.View;
			}
		 	else if (vc is UITabBarController)
			{
				_tabBarController = (vc as UITabBarController);
				this.View = vc.View;
			}
			else 
			{ 
				// we are an iPhone, skip the split view and use the navigation controller instead!
				this.View = _masterNavigationController.View;
			}				
		}
		
		public void DisplayViewInTabBar(UIViewController vc, bool animated)
		{
			var navCtrl = (UINavigationController)_tabBarController.ViewControllers[_tabBarController.SelectedIndex];
			
			if (navCtrl.ViewControllers.Contains(vc)) { navCtrl.PopToViewController(vc, true); }
			else { navCtrl.PushViewController(vc, true);	}
		}

        public void NavigateToPopover(UIViewController vc, bool animated)
        {
            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
            {

            }
            else
            {
                _masterNavigationController.PresentModalViewController(vc, animated);
            }
        }

		public void NavigateToMaster(UIViewController vc)
		{
			// first time through!
			if (_splitViewController == null && _masterNavigationController == null)
			{
				Init(vc);
			}
			else
			{
				bool contains = _masterNavigationController.ViewControllers.Contains(vc);
				if (contains) { _masterNavigationController.PopToViewController(vc, true); }
				else { _masterNavigationController.PushViewController(vc, true); }
			}
		}
		
		public void NavigateToDetail(UIViewController vc)
		{
			// first time through!
			if (_splitViewController == null && _masterNavigationController == null)
			{
				Init(vc);
				return;
			}

			if (_splitViewController != null)
			{
				// ipad -- set pane, we just change out the panes depending on the master
				_detailNavigationController.PopViewControllerAnimated(false);
				_detailNavigationController.PushViewController(vc, false);
				_splitViewControllerDelegate.ReplaceDetailNavigationViewController();
				_splitViewControllerDelegate.HidePopover();
			}
			else
			{
				_masterNavigationController.PushViewController(vc, true);
			}
		}
		
		public void NavigateToDetail(UIViewController vc, MXTouchAnimation direction)
		{
			CATransition transition = new CATransition();
			
			transition.Duration = 0.5;
			transition.Type = CATransition.TransitionMoveIn;
			if (direction == MXTouchAnimation.Left)
				transition.Subtype = CATransition.TransitionFromRight;
			else
			    transition.Subtype = CATransition.TransitionFromLeft;
				
			if (_splitViewController != null)
			{
				// ipad -- set pane, we just change out the panes depending on the master
				_detailNavigationController.PopViewControllerAnimated(false);

				_detailNavigationController.View.Layer.AddAnimation(transition, CALayer.Transition);
				_detailNavigationController.PushViewController(vc, false);

				_splitViewControllerDelegate.ReplaceDetailNavigationViewController();
				_splitViewControllerDelegate.HidePopover();
			}
			else
			{
				_masterNavigationController.PopViewControllerAnimated(false);

				_masterNavigationController.View.Layer.AddAnimation(transition, CALayer.Transition);
				_masterNavigationController.PushViewController(vc, false);
			}
		}
	}
#endif
}
