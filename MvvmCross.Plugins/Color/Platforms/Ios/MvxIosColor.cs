// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.UI;
using UIKit;

namespace MvvmCross.Plugin.Color.Platforms.Ios
{
    [Preserve(AllMembers = true)]
    public class MvxIosColor : IMvxNativeColor
    {
        public object ToNative(System.Drawing.Color color)
        {
            return ToUIColor(color);
        }

        public static UIColor ToUIColor(System.Drawing.Color color)
        {
            return new UIColor(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
        }
    }
}
