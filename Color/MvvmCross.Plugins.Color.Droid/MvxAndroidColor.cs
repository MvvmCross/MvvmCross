// MvxAndroidColor.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform.UI;

namespace MvvmCross.Plugins.Color.Droid
{
    [Preserve(AllMembers = true)]
    public class MvxAndroidColor : IMvxNativeColor
    {
        public object ToNative(MvxColor mvxColor)
        {
            return ToAndroidColor(mvxColor);
        }

        public global::Android.Graphics.Color ToAndroidColor(MvxColor mvxColor)
        {
            return new global::Android.Graphics.Color(mvxColor.R, mvxColor.G, mvxColor.B, mvxColor.A);
        }
    }
}