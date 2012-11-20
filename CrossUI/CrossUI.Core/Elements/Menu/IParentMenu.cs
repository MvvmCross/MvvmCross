using System.Collections.Generic;

namespace CrossUI.Core.Elements.Menu
{
    public interface IParentMenu : IMenu
    {
        List<IMenu> Children { get; }
    }
}