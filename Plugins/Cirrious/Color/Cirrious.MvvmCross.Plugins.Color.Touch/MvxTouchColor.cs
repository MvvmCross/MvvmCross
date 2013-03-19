// MvxTouchColor.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.UI;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Plugins.Color.Touch
{
    public class MvxTouchColor : IMvxNativeColor
    {
        public object ToNative(MvxColor mvxColor)
        {
            return ToUIColor(mvxColor);
        }

        public static UIColor ToUIColor(MvxColor mvxColor)
        {
            return new MonoTouch.UIKit.UIColor(mvxColor.R, mvxColor.G, mvxColor.B, mvxColor.A);
        }
    }
}