// MvxTouchColorExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MonoTouch.UIKit;

namespace Cirrious.CrossCore.Touch
{
    public static class MvxTouchColorExtensionMethods
    {
        public static UIColor ColorFromInt(this uint rgbValue)
        {
            float red = (rgbValue & 0xFF0000) >> 16;
            float green = (rgbValue & 0xFF00) >> 8;
            float blue = rgbValue & 0xFF;
            return UIColor.FromRGB(red/255, green/255, blue/255);
        }

        public static UIColor ColorWithAlphaFromInt(this uint rgbaValue)
        {
            float red = (rgbaValue & 0xFF0000) >> 16;
            float green = (rgbaValue & 0xFF00) >> 8;
            float blue = rgbaValue & 0xFF;
            float alpha = (rgbaValue & 0xFF000000) >> 24;
            return UIColor.FromRGBA(red/255, green/255, blue/255, alpha/255);
        }

        public static uint IntFromColor(this UIColor color)
        {
            float red, green, blue, alpha;
            color.GetRGBA(out red, out green, out blue, out alpha);
            var rgbaValue = (uint) (((long) alpha) << 24 | ((long) red) << 16 | ((long) green) << 8 | ((long) blue));
            return rgbaValue;
        }
    }
}