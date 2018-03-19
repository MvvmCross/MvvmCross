// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Windows.Media;
using MvvmCross.UI;

namespace MvvmCross.Plugin.Color.Platform.Wpf
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
