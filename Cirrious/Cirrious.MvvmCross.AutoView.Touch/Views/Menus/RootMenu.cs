using System.Collections.Generic;
using CrossUI.Core.Elements.Menu;

namespace Cirrious.MvvmCross.AutoView.Touch.Views.Menus
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