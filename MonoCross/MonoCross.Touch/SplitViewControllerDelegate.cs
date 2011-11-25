
using System;
using MonoTouch.UIKit;

namespace MonoCross.Touch
{
	internal class SplitViewController : MGSplitViewController // UISplitViewController
	{
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}
	}
	
	/*
	
	internal class SplitViewControllerDelegate: UISplitViewControllerDelegate
	{
		private SplitViewController _svc;
		private UIPopoverController _pc;
		private UIBarButtonItem _lefty;

		public SplitViewControllerDelegate ()
		{
			_lefty = null;
			_pc = null;
		}
		
		public void HidePopover()
		{
			if (_pc != null)
				_pc.Dismiss(true);
		}
		
		public void ReplaceDetailNavigationViewController()
		{
			if (_svc != null)
			{
				UINavigationController nc = _svc.ViewControllers[1] as UINavigationController;
				nc.NavigationItem.LeftBarButtonItem = _lefty;
				nc.VisibleViewController.NavigationItem.LeftBarButtonItem = _lefty;
			}
		}
		
		public override void WillHideViewController (UISplitViewController svc, UIViewController aViewController, UIBarButtonItem barButtonItem, UIPopoverController pc)
		{
			_svc = svc as SplitViewController;
			_lefty = barButtonItem;
			_lefty.Title = "Home";
			_pc = pc;
			ReplaceDetailNavigationViewController();
		}
		
		public override void WillShowViewController (UISplitViewController svc, UIViewController aViewController, UIBarButtonItem button)
		{
			_pc = null;
			_lefty = null;

			ReplaceDetailNavigationViewController();
		}
		
		public override void WillPresentViewController (UISplitViewController svc, UIPopoverController pc, UIViewController aViewController)
		{
			if (pc != null)
				pc.Dismiss(true);
		}
	}
	
	*/
}

