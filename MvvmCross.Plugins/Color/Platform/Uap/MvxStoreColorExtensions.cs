// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Base.UI;

namespace MvvmCross.Plugin.Color.Platform.Uap
{
    public static class MvxStoreColorExtensions
    {
        public static Windows.UI.Color ToNativeColor(this MvxColor mvxColor)
        {
            var color = Windows.UI.Color.FromArgb((byte)mvxColor.A,
                                                  (byte)mvxColor.R,
                                                  (byte)mvxColor.G,
                                                  (byte)mvxColor.B);
            return color;
        }
    }
}
