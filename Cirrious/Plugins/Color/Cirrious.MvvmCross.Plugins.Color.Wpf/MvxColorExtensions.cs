namespace Cirrious.MvvmCross.Plugins.Color.Wpf
{
    public static class MvxColorExtensions
    {
        public static System.Windows.Media.Color ToNativeColor(this MvxColor color)
        {
            return MvxWpfColor.ToNativeColor(color);
        }
    }
}