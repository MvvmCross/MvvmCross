using System.Windows.Input;
using Foobar.Dialog.Core.Menus;

namespace FooBar.Dialog.Droid.Menus
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