// HackMvxMenuExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Cirrious.MvvmCross.AutoView.Touch.Views.Menus;
using CrossUI.Core.Elements.Menu;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.AutoView.Touch.ExtensionMethods
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
                    if (e.ButtonIndex >= 0)
                    {
                        actions[e.ButtonIndex].Execute(null);
                    }
                };

#warning More to do here - e.g. check for null!
            //if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
            //	actionSheet.ShowFromToolbar(NavigationController.Toolbar);
            //else
            actionSheet.ShowFrom(vc.NavigationItem.RightBarButtonItem, true);
        }
    }
}