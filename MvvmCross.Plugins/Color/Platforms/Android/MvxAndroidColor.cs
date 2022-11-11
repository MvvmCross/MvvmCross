// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.UI;

namespace MvvmCross.Plugin.Color.Platforms.Android
{
    [Preserve(AllMembers = true)]
    public class MvxAndroidColor : IMvxNativeColor
    {
        public object ToNative(System.Drawing.Color color)
        {
            return ToNativeColor(color);
        }

        public global::Android.Graphics.Color ToNativeColor(System.Drawing.Color color)
        {
            return new global::Android.Graphics.Color(color.R, color.G, color.B, color.A);
        }
    }
}
