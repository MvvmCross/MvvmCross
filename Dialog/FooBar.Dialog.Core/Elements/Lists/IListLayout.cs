using System.Collections.Generic;
using Foobar.Dialog.Core.Descriptions;

namespace Foobar.Dialog.Core.Lists
{
    public interface IListLayout : IBaseListLayout
    {
        IListItemLayout DefaultLayout { get; }
        Dictionary<string, IListItemLayout> ItemLayouts { get; }
    }
}