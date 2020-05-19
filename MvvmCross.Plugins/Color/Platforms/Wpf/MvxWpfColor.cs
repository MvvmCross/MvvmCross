// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Windows.Media;
using MvvmCross.UI;

namespace MvvmCross.Plugin.Color.Platforms.Wpf
{
    public class MvxWpfColor : IMvxNativeColor
    {
        public object ToNative(System.Drawing.Color color)
        {
            var wpfColor = ToNativeColor(color);
            return new SolidColorBrush(wpfColor);
        }

        public static System.Windows.Media.Color ToNativeColor(System.Drawing.Color mvxColor)
        {
            return System.Windows.Media.Color.FromArgb(mvxColor.A, mvxColor.R, mvxColor.G, mvxColor.B);
        }
    }
}
