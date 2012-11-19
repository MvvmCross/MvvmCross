using System.Collections.Generic;

namespace CrossUI.Core.Descriptions.Lists
{
    public class ListLayoutDescription : BaseListDescription
    {
        public ListItemLayoutDescription DefaultLayout { get; set; }
        public Dictionary<string, ListItemLayoutDescription> ItemLayouts { get; set; }
    }
}