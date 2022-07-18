// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.UI.Xaml.Media;
using MvvmCross.UI;

namespace MvvmCross.Plugin.Color.Platforms.WinUi
{
    public class MvxWindowsColor : IMvxNativeColor
    {
        public object ToNative(System.Drawing.Color color)
        {
            var nativeColor = color.ToNativeColor();
            return new SolidColorBrush(nativeColor);
        }
    }
}
