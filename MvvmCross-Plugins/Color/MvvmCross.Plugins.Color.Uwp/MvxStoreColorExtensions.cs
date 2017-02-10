// MvxStoreColorExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform.UI;

namespace MvvmCross.Plugins.Color.WindowsCommon
{
    public static class MvxStoreColorExtensions
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