using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foobar.Dialog.Core.Elements;
using Foobar.Dialog.Core.Menus;
using IMenu = Foobar.Dialog.Core.Menus.IMenu;

namespace FooBar.Dialog.Touch.Menus
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