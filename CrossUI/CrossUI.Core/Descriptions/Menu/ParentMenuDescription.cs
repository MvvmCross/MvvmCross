// ParentMenuDescription.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;

namespace CrossUI.Core.Descriptions.Menu
{
    public class ParentMenuDescription : MenuDescription
    {
        public List<MenuDescription> Children { get; set; }

        public ParentMenuDescription()
        {
            Children = new List<MenuDescription>();
        }
    }
}