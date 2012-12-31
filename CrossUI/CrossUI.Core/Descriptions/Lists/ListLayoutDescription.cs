#region Copyright

// <copyright file="ListLayoutDescription.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System.Collections.Generic;

namespace CrossUI.Core.Descriptions.Lists
{
    public class ListLayoutDescription : BaseListDescription
    {
        public ListItemLayoutDescription DefaultLayout { get; set; }
        public Dictionary<string, ListItemLayoutDescription> ItemLayouts { get; set; }
    }
}