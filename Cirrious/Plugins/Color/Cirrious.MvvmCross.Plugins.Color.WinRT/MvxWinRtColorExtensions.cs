namespace Cirrious.MvvmCross.Plugins.Color.WinRT
{
    public static class MvxWinRtColorExtensions
    {
        public static Windows.UI.Color ToNativeColor(this MvxColor mvxColor)
        {
            var color = Windows.UI.Color.FromArgb((byte)mvxColor.A,
                                                  (byte)mvxColor.R,
                                                  (byte)mvxColor.G,
                                                  (byte)mvxColor.B);
            return color;
        }
    }
}