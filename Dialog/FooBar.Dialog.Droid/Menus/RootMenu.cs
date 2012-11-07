using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Foobar.Dialog.Core.Elements;
using Foobar.Dialog.Core.Menus;
using IMenu = Foobar.Dialog.Core.Menus.IMenu;

namespace FooBar.Dialog.Droid.Menus
{
    public class RootMenu : IParentMenu
    {
        public List<IMenu> Children { get; private set; }

        public RootMenu()
        {
            Children = new List<IMenu>();
        }
    }
}