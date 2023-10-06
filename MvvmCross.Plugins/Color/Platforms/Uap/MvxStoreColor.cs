// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.UI;
using Windows.UI.Xaml.Media;

namespace MvvmCross.Plugin.Color.Platforms.Uap
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
