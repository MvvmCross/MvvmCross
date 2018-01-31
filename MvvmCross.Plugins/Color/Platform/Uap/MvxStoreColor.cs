// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml.Media;
using MvvmCross.Platform.UI;

namespace MvvmCross.Plugins.Color.Uwp
{
    public class MvxWindowsCommonColor : IMvxNativeColor
    {
        public object ToNative(MvxColor mvxColor)
        {
            var color = mvxColor.ToNativeColor();
            return new SolidColorBrush(color);
        }
    }
}