using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Cirrious.MvvmCross.WindowsCommon.Views.Helpers
{
    public static class Keyboard
    {
        public static bool IsControlKeyDown
        {
            get { return IsKeyDown(VirtualKey.Control); }
        }

        public static bool IsShiftKeyDown
        {
            get { return IsKeyDown(VirtualKey.Shift); }
        }

        public static bool IsAltKeyDown
        {
            get { return IsKeyDown(VirtualKey.LeftMenu); }
        }

        public static bool IsKeyDown(VirtualKey key)
        {
            return (Window.Current.CoreWindow.GetKeyState(key) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
        }
    }
}
