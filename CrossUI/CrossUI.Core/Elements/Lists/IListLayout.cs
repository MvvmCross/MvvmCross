using System.Collections.Generic;

namespace CrossUI.Core.Elements.Lists
{
    public interface IListLayout : IBaseListLayout
    {
        IListItemLayout DefaultLayout { get; }
        Dictionary<string, IListItemLayout> ItemLayouts { get; }
    }
}