#region Copyright

// <copyright file="HackMvxMenuExtensionMethods.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System.Collections.Generic;
using System.Windows.Input;
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