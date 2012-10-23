using System.Collections.Generic;

namespace Foobar.Dialog.Core.Menus
{
    public interface IParentMenu : IMenu
    {
        List<IMenu> Children { get; }
    }
}