// CaptionAndIconMenu.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using CrossUI.Core.Elements.Menu;
using System.Windows.Input;

namespace CrossUI.Droid.Menus
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