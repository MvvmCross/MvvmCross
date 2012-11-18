using Foobar.Dialog.Core.Menus;
using FooBar.Dialog.Touch.Menus;
using MonoTouch.UIKit;
using System.Collections.Generic;
using System.Windows.Input;

namespace Cirrious.MvvmCross.AutoView.Droid.ExtensionMethods
{
    public static class HackMvxMenuExtensionMethods
    {
        public static void ShowOptionsMenu(this UIViewController vc, IParentMenu parentMenu)
        {
            if (parentMenu == null)
            {
                return;
            }

			var actionSheet = new UIActionSheet();

#warning TODO - make this OO - let the _parentMenu render itself...
			var actions = new List<ICommand>();
			foreach (var child in parentMenu.Children)
            {
                var childCast = child as CaptionAndIconMenu;

#warning More to do here - e.g. check for null!
				actionSheet.AddButton(childCast.Caption);
				actions.Add(childCast.Command);
            }

			actionSheet.Clicked += (object sender, UIButtonEventArgs e) => 
			{
				actions[e.ButtonIndex].Execute(null);
			};         

#warning More to do here - e.g. check for null!
			//if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
			//	actionSheet.ShowFromToolbar(NavigationController.Toolbar);
			//else
			actionSheet.ShowFrom(vc.NavigationItem.RightBarButtonItem, true);
	    }
    }
}