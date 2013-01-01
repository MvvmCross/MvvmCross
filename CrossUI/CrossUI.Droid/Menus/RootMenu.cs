// RootMenu.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using CrossUI.Core.Elements.Menu;

namespace CrossUI.Droid.Menus
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