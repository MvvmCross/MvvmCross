// MvxTouchColor.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.UI;
using UIKit;

namespace MvvmCross.Plugins.Color.iOS
{
    public class MvxTouchColor : IMvxNativeColor
    {
        public object ToNative(MvxColor mvxColor)
        {
            return ToUIColor(mvxColor);
        }

        public static UIColor ToUIColor(MvxColor mvxColor)
        {
            return new UIColor(mvxColor.R / 255.0f, mvxColor.G / 255.0f, mvxColor.B / 255.0f, mvxColor.A / 255.0f);
        }
    }
}