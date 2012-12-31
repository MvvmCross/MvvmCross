#region Copyright

// <copyright file="CaptionAndIconMenu.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System.Windows.Input;
using CrossUI.Core.Elements.Menu;

namespace Cirrious.MvvmCross.AutoView.Touch.Views.Menus
{
    public class CaptionAndIconMenu : IMenu
    {
        private static int _menuCounter = 1;

        public CaptionAndIconMenu()
        {
            UniqueId = _menuCounter++;
        }

        public int UniqueId { get; private set; }
        public string Caption { get; set; }
        public string LongCaption { get; set; }
        public string Icon { get; set; }
        public ICommand Command { get; set; }
    }
}