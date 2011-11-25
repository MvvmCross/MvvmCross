using System;
using MonoTouch.UIKit;
using MonoCross.Navigation;
using System.Collections.Generic;

namespace MonoCross.Touch
{
	/// <summary>
	/// MX touch view group item.
	/// </summary>
	public class MXTouchViewGroupItem
	{
		public MXTouchViewGroupItem(Type viewType, String title, String iconFile)
		{
			this.ViewType = viewType;
			this.View = null;
			this.Title = title;
			this.Icon = iconFile;
		}

		public MXTouchViewGroupItem(IMXView view, String title, String iconFile)
		{
			this.ViewType = view.GetType();
			this.View = view;
			this.Title = title;
			this.Icon = iconFile;
		}
		
		public IMXView View { get; set; }
		public Type ViewType { get; private set; }
		public String Icon { get; set; }
		public String Title { get; set; }
	}
	
	/// <summary>
	/// MX touch view group.
	/// </summary>
	public class MXTouchViewGroup
	{
		public MXTouchViewGroup(IMXTouchViewGroupController controller, MXTouchViewGroupItem[] items)
		{
			ViewController = controller;
			Items = new List<MXTouchViewGroupItem>();
			Items.AddRange(items);
		}
		
		public MXTouchViewGroup(IMXTouchViewGroupController controller)
		{
			ViewController = controller;
			Items = new List<MXTouchViewGroupItem>();
		}

		public IMXTouchViewGroupController ViewController { get; private set; }
		public List<MXTouchViewGroupItem> Items { get; private set; }
	}

	/// <summary>
	/// View Group base functionality, dictates how the view group behaves
	/// </summary>
	public interface IMXTouchViewGroupController
	{
		void Render(MXTouchViewGroup viewGroup);
		void RenderItem(int groupIndex, UIViewController viewController);
	}

	/// <summary>
	/// MX touch view group tab controller.
	/// </summary>
	public class MXTouchViewGroupTabController : UITabBarController, IMXTouchViewGroupController
	{
		internal MXTouchViewGroup _viewGroup;
		
		public MXTouchViewGroupTabController()
		{
			this.Delegate = new TabBarControllerDelegate(this);
		}

		public void Render(MXTouchViewGroup viewGroup)
		{
			if (_viewGroup != null)
				// already rendered
				return;
			
			_viewGroup = viewGroup;
			
			System.Console.WriteLine("MXTouchViewGroupTabController: Render");
			
			this.Delegate = new TabBarControllerDelegate(this);

			var tabControllers = new UIViewController[viewGroup.Items.Count];

			int index = 0;
			foreach (var tabItem in viewGroup.Items) {
				var navCtrl = new UINavigationController();

				navCtrl.NavigationBar.TintColor = UIColor.Red;
				
				//navCtrl.NavigationBar.TintColor = UIColor.Black; ?? get from where??
				navCtrl.TabBarItem = new UITabBarItem (tabItem.Title, UIImage.FromFile(tabItem.Icon), index);
				tabControllers[index] = navCtrl;
				index++;
			}
			
			SetViewControllers(tabControllers, true);
		}
		
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			
			// hide the navigation bar that the tabs are in, child views can have them if needed.
			if (this.NavigationController != null)
				this.NavigationController.NavigationBarHidden = true;
		}

		
		public void RenderItem(int index, UIViewController viewController)
		{
			if (SelectedIndex != index)
				SelectedIndex = index;
			
			UINavigationController navController = ViewControllers[SelectedIndex] as UINavigationController;
			//UINavigationControllerExtensions.DisplayViewController(ViewControllers[SelectedIndex] as UINavigationController, viewController, true);
			navController.DisplayViewController(viewController, true);
		}
		
		private class TabBarControllerDelegate: UITabBarControllerDelegate
		{
			MXTouchViewGroupTabController _parent;

			public TabBarControllerDelegate(MXTouchViewGroupTabController parent)
			{
				_parent = parent;
			}
			
			public override bool ShouldSelectViewController (UITabBarController tabBarController, UIViewController viewController)
			{
				UINavigationController navController = viewController as UINavigationController;
				System.Console.WriteLine("TabBarControllerDelegate:ShouldSelectViewController");

				if (navController.ViewControllers.Length == 0)
				{
					int index = Array.IndexOf(tabBarController.ViewControllers, viewController);
					if (index >= 0)
					{
						Type viewType = _parent._viewGroup.Items[index].ViewType;
						
						MXViewPerspective viewPerspective = MXContainer.Instance.Views.GetViewPerspectiveForViewType(viewType);
						string pattern = MXContainer.Instance.App.NavigationMap.GetPatternForModelType(viewPerspective.ModelType);
						MXTouchContainer.Navigate(null, pattern);
					}
				}
				else
				{
					// do nothing, tab has a view, leave it for the contained view to figure out what needs
					// to be done
				}
				
				return true;
			}
			public override void ViewControllerSelected (UITabBarController tabBarController, UIViewController viewController)
			{
				System.Console.WriteLine("TabBarControllerDelegate:ViewControllerSelected");
				//base.ViewControllerSelected(tabBarController, viewController);
			}
			
			//public override void OnCustomizingViewControllers (UITabBarController tabBarController, UIViewController[] viewControllers)
			//{
			//	System.Console.WriteLine("TabBarControllerDelegate:OnCustomizingViewControllers");
			//	//base.OnCustomizingViewControllers(tabBarController, viewControllers);
			//}
			//public override void OnEndCustomizingViewControllers (UITabBarController tabBarController, UIViewController[] viewControllers, bool changed)
			//{
			//	System.Console.WriteLine("TabBarControllerDelegate:OnEndCustomizingViewControllers");
			//	//base.OnEndCustomizingViewControllers(tabBarController, viewControllers, changed);
			//}
			//public override void FinishedCustomizingViewControllers (UITabBarController tabBarController, UIViewController[] viewControllers, bool changed)
			//{
			//	System.Console.WriteLine("TabBarControllerDelegate:FinishedCustomizingViewControllers");
			//	//base.FinishedCustomizingViewControllers(tabBarController, viewControllers, changed);
			//}
		}
	}
}

