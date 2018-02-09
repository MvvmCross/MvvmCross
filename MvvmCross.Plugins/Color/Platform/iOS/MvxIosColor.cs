// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.UI;
using UIKit;

namespace MvvmCross.Plugin.Color.Platform.iOS
{
    [Preserve(AllMembers = true)]
    public class MvxIosColor : IMvxNativeColor
    {
        public object ToNative(MvxColor mvxColor)
        {
            return ToUIColor(mvxColor);
        }

        public static UIColor ToUIColor(MvxColor mvxColor)
        {
            return new UIColor(mvxColor.R / 255.0f, mvxColor.G / 255.0f, mvxColor.B / 255.0f, mvxColor.A / 255.0f);
        }
    }
}
