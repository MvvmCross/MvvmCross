// MvxWpfColor.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.UI;
using System.Windows.Media;

namespace MvvmCross.Plugins.Color.Wpf
{
    public class MvxWpfColor : IMvxNativeColor
    {
        public object ToNative(MvxColor mvxColor)
        {
            var color = ToNativeColor(mvxColor);
            return new SolidColorBrush(color);
        }

        public static System.Windows.Media.Color ToNativeColor(MvxColor mvxColor)
        {
            return System.Windows.Media.Color.FromArgb((byte)mvxColor.A, (byte)mvxColor.R, (byte)mvxColor.G,
                                                       (byte)mvxColor.B);
        }
    }
}