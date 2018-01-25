// MvxAndroidColor.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform;
using MvvmCross.Platform.UI;

namespace MvvmCross.Plugins.Color.Droid
{
    [Preserve(AllMembers = true)]
    public class MvxAndroidColor : IMvxNativeColor
    {
        public object ToNative(MvxColor mvxColor)
        {
            return ToNativeColor(mvxColor);
        }

        public Android.Graphics.Color ToNativeColor(MvxColor mvxColor)
        {
            return new Android.Graphics.Color(mvxColor.R, mvxColor.G, mvxColor.B, mvxColor.A);
        }
    }
}
