// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Base.UI;
using UIKit;

namespace MvvmCross.Plugin.Color.Platform.iOS
{
    public static class MvxColorExtensions
    {
        public static UIColor ToNativeColor(this MvxColor color)
        {
            return MvxIosColor.ToUIColor(color);
        }
    }
}
