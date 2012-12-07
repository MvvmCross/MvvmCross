using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Plugins.Color.Touch
{
    public static class MvxColorExtensions
    {
        public static UIColor ToAndroidColor(this MvxColor color)
        {
            return MvxTouchColor.ToUIColor(color);
        }
    }
}