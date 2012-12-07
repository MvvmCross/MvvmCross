namespace Cirrious.MvvmCross.Plugins.Color.WindowsPhone
{
    public static class MvxColorExtensions
    {
        public static System.Windows.Media.Color ToNativeColor(this MvxColor color)
        {
            return MvxWindowsPhoneColor.ToNativeColor(color);
        }
    }
}