// MvxColorExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.UI;

namespace MvvmCross.Plugins.Color.Droid
{
    public static class MvxColorExtensions
    {
        private static readonly MvxAndroidColor _mvxNativeColor = new MvxAndroidColor();

        public static global::Android.Graphics.Color ToAndroidColor(this MvxColor color)
        {
            return _mvxNativeColor.ToAndroidColor(color);
        }
    }
}