// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Plugin.Color.Platforms.Wpf
{
    public static class MvxColorExtensions
    {
        public static System.Windows.Media.Color ToNativeColor(this System.Drawing.Color color)
        {
            return MvxWpfColor.ToNativeColor(color);
        }
    }
}
