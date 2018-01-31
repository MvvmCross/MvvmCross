// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Platform;
using MvvmCross.Platform.UI;

namespace MvvmCross.Plugins.Color.Droid
{
    [Preserve(AllMembers = true)]
    public class MvxAndroidColor : IMvxNativeColor
    {
        public object ToNative(MvxColor mvxColor)
        {
            return ToNativeColor(mvxColor);
        }

        public Android.Graphics.Color ToNativeColor(MvxColor mvxColor)
        {
            return new Android.Graphics.Color(mvxColor.R, mvxColor.G, mvxColor.B, mvxColor.A);
        }
    }
}
