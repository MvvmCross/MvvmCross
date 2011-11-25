using System;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreAnimation;
using System.Linq;

namespace MonoCross.Touch
{
	public enum MXTouchAnimation
	{
		Left,
		Right
	}
	
    /// <summary>
    ///
    /// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class TouchViewOptions: System.Attribute
	{
		public ViewNavigationContext ViewDest { get; set; }
		public string Icon { get; set; }
		public bool ViewInTab { get; set; }

		public TouchViewOptions(ViewNavigationContext viewdest)
		{
			ViewDest = viewdest;
		}
	}
	
	public static class UINavigationControllerExtensions
	{
		public static bool DisplayViewController(this UINavigationController navController, UIViewController viewController, bool animate)
		{
			// Crashed with error in Linq, 
			//UIViewController found = navController.ViewControllers.First(v => v == viewController);
			bool found = false;
			foreach (var vc in navController.ViewControllers) {
				if (vc == viewController) {
					found = true;
					break;
				}
			}
			
			if (found)
				navController.PopToViewController(viewController, animate);
			else
				navController.PushViewController(viewController, animate);

			return true;
		}
	}

	
	public class MXTouchNavigation
	{
		static MXTouchNavigation _instance;
		
		public MXTouchNavigation (UIApplicationDelegate appDelegate)
		{
			_instance = this;
			
			var options = Attribute.GetCustomAttribute(appDelegate.GetType(), typeof(MXTouchContainerOptions)) as MXTouchContainerOptions;
			_options = options ?? new MXTouchContainerOptions();

			var tabletOptions = Attribute.GetCustomAttribute(appDelegate.GetType(), typeof(MXTouchTabletOptions)) as MXTouchTabletOptions;
			_tabletOptions = tabletOptions ?? new MXTouchTabletOptions(TabletLayout.SinglePane);
		}
		
		public UINavigationController MasterNavigationController { get { return _masterNavigationController; } }
		public UINavigationController DetailNavigationController { get { return _detailNavigationController; } }
		
		public UIView View { get; set; }

		public static bool IsTablet { get {return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad;} }

		
		internal MXTouchContainerOptions ContainerOptions
		{
			get { return _options; }
		}
		
		internal MXTouchTabletOptions TabletOptions
		{
			get { return _tabletOptions; }
		}
		
		public static bool MasterDetailLayout
		{
			get
			{
				return IsTablet && _instance.TabletOptions.TabletLayout == TabletLayout.MasterPane;
			}
		}

		
		MXTouchContainerOptions _options;
		MXTouchTabletOptions _tabletOptions;
		
		// when on iPad
		MGSplitViewController _splitViewController = null;
		UINavigationController _detailNavigationController = null;

		// detail pane or master when on when on iPod Touch/iPhone
		UINavigationController _masterNavigationController = null;
		
		// temporary view in the detail pane if needed
		SplashViewController _splashViewController = null;
		
		
		public void Init(UIViewController viewController)
		{
			_masterNavigationController = new UINavigationController(viewController);

			if (IsTablet && _tabletOptions.TabletLayout == TabletLayout.MasterPane)
			{
				// toss a generic view in the detail pane
				_splashViewController = new SplashViewController(_options.SplashBitmap);
				_detailNavigationController = new UINavigationController(_splashViewController);
				
				// initialize the SplitPane
				_splitViewController = new MGSplitViewController();
				_splitViewController.SetViewControllers(new UIViewController[2] { _masterNavigationController, _detailNavigationController });
				_splitViewController.SetShowsMasterInPortrait(_tabletOptions.MasterShowsinPotrait);
				_splitViewController.SetShowsMasterInLandscape(_tabletOptions.MasterShowsinLandscape);
				_splitViewController.SetMasterBeforeDetail(_tabletOptions.MasterBeforeDetail);
				if (_tabletOptions.AllowDividerResize)
					_splitViewController.SetDividerStyle(MGSplitViewDividerStyle.PaneSplitter);
				else 
					_splitViewController.SetDividerStyle(MGSplitViewDividerStyle.Thin);
				
				// primary view with be the split view
				this.View = _splitViewController.View;
				
				if (_options.NavigationBarTintColor != UIColor.Clear)
				{
					_masterNavigationController.NavigationBar.TintColor = _options.NavigationBarTintColor;
					_detailNavigationController.NavigationBar.TintColor = _options.NavigationBarTintColor;
				}
			}
			else 
			{ 
				// we are an iPhone, skip the split view and use the navigation controller instead!
				this.View = _masterNavigationController.View;
			}				
		}
		
		public void PushToViewGroup(MXTouchViewGroup viewGroup, MXTouchViewGroupItem viewGroupItem, UIViewController viewController)
		{
			// let the group render itself if it needs to
			viewGroup.ViewController.Render(viewGroup);
			UIViewController viewGroupViewController = viewGroup.ViewController as UIViewController;

			// put the item in the proper view group
			int groupIndex = viewGroup.Items.FindIndex( vgi => vgi == viewGroupItem );
			viewGroup.ViewController.RenderItem(groupIndex, viewController);

			// only support the master for now
			if (_splitViewController == null && _masterNavigationController == null)
			{
				// first time through!
				Init(viewGroupViewController);
			}
			else
			{
				_masterNavigationController.DisplayViewController(viewGroupViewController, true);
			}
		}
		
		// place view in the master pane
		public void PushToMaster(UIViewController vc)
		{
			if (_splitViewController == null && _masterNavigationController == null)
			{
				// first time through!
				Init(vc);
			}
			else
			{
				_masterNavigationController.DisplayViewController(vc, true);
			}
		}

		// place this item in the detail pane
		public void PushToDetail(UIViewController vc)
		{
			// first time through!
			if (_splitViewController == null && _masterNavigationController == null)
			{
				Init(vc);
				return;
			}

			if (_splitViewController != null)
			{
				_splitViewController.HidePopover();
				_detailNavigationController.SetViewControllers(new UIViewController[1] { vc }, false);
				//_splitViewController.SetDetailViewController(_detailNavigationController = new UINavigationController(vc));
				_splitViewController.UpdateMasterButton();
				
				//if (_options.NavigationBarTintColor != UIColor.Clear)
				//{
				//	_detailNavigationController.NavigationBar.TintColor = _options.NavigationBarTintColor;
				//}


				// ipad -- set pane, we just change out the panes depending on the master
				//_detailNavigationController.PopToRootViewController(false);
				//_detailNavigationController.PopViewControllerAnimated(false);
				//_detailNavigationController.PushViewController(vc, false);
				//_splitViewControllerDelegate.ReplaceDetailNavigationViewController();
				//_splitViewControllerDelegate.HidePopover();
			}
			else
			{
				_masterNavigationController.DisplayViewController(vc, true);

			}
		}
		
		public void PushToModel(UIViewController vc)
		{
			if (_splitViewController != null)
			{
				_splitViewController.PresentModalViewController(vc, true);
			}
			else
			{
				_masterNavigationController.PresentModalViewController(vc, true);
			}
		}
		
		/*
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

				//_splitViewControllerDelegate.ReplaceDetailNavigationViewController();
				//_splitViewControllerDelegate.HidePopover();
			}
			else
			{
				_masterNavigationController.PopViewControllerAnimated(false);

				_masterNavigationController.View.Layer.AddAnimation(transition, CALayer.Transition);
				_masterNavigationController.PushViewController(vc, false);
			}
		}
		*/
		
		public static ViewNavigationContext GetViewNavigationContext(object view)
		{
			// Show MyCustomAttribute information for the testClass
			var attr = Attribute.GetCustomAttribute(view.GetType(), typeof(MXTouchViewAttributes)) as MXTouchViewAttributes;
			if (attr != null)
				return attr.NavigationContext;
			else
				return ViewNavigationContext.Detail;
		}
	}
}

