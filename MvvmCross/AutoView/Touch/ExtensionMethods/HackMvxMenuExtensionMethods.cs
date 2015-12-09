// HackMvxMenuExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Touch.ExtensionMethods
{
    using System.Collections.Generic;
    using System.Windows.Input;

    using CrossUI.Core.Elements.Menu;

    using MvvmCross.AutoView.Touch.Views.Menus;

    using UIKit;

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
                actionSheet.AddButton(childCast?.Caption);
                actions.Add(childCast?.Command);
            }

            actionSheet.Clicked += (object sender, UIButtonEventArgs e) =>
                {
                    if (e.ButtonIndex >= 0)
                    {
                        actions[(int)e.ButtonIndex].Execute(null);
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