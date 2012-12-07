using System.Collections.Generic;
using CrossUI.Core.Elements.Menu;
using IMenu = CrossUI.Core.Elements.Menu.IMenu;

namespace CrossUI.Droid.Menus
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