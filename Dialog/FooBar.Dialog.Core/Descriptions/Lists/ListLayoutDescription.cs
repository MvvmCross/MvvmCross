using System;
using System.Collections;
using System.Collections.Generic;
using Foobar.Dialog.Core.Lists;

namespace Foobar.Dialog.Core.Descriptions
{
    public class ListLayoutDescription : BaseListDescription
    {
        public ListItemLayoutDescription DefaultLayout { get; set; }
        public Dictionary<string, ListItemLayoutDescription> ItemLayouts { get; set; }
    }
}