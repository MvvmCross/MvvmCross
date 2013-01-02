// MvxWindowsPhoneColor.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Windows.Media;

namespace Cirrious.MvvmCross.Plugins.Color.WindowsPhone
{
    public class MvxWindowsPhoneColor : IMvxNativeColor
    {
        #region Implementation of IMvxNativeColor

        public object ToNative(MvxColor mvxColor)
        {
            var color = ToNativeColor(mvxColor);
            return new SolidColorBrush(color);
        }

        #endregion

        public static System.Windows.Media.Color ToNativeColor(MvxColor mvxColor)
        {
            return System.Windows.Media.Color.FromArgb((byte) mvxColor.A, (byte) mvxColor.R, (byte) mvxColor.G,
                                                       (byte) mvxColor.B);
        }
    }
}