// ListLayoutDescription.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;

namespace CrossUI.Core.Descriptions.Lists
{
    public class ListLayoutDescription : BaseListDescription
    {
        public ListItemLayoutDescription DefaultLayout { get; set; }
        public Dictionary<string, ListItemLayoutDescription> ItemLayouts { get; set; }
    }
}