// MvxColorExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.UI;
using UIKit;

namespace MvvmCross.Plugins.Color.Touch
{
    public static class MvxColorExtensions
    {
        public static UIColor ToNativeColor(this MvxColor color)
        {
            return MvxTouchColor.ToUIColor(color);
        }
    }
}