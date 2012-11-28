namespace Cirrious.MvvmCross.Plugins.Color.Droid
{
    public static class MvxColorExtensions
    {
        private static MvxAndroidColor _mvxNativeColor = new MvxAndroidColor();

        public static global::Android.Graphics.Color ToAndroidColor(this MvxColor color)
        {
            return _mvxNativeColor.ToAndroidColor(color);
        }
    }
}