// RootMenu.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Touch.Views.Menus
{
    using System.Collections.Generic;

    using CrossUI.Core.Elements.Menu;

    public class RootMenu : IParentMenu
    {
        public List<IMenu> Children { get; private set; }

        public RootMenu()
        {
            this.Children = new List<IMenu>();
        }
    }
}