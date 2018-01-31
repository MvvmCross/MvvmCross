// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Platform.UI;

namespace MvvmCross.Plugins.Color.Droid
{
    public static class MvxColorExtensions
    {
        private static readonly MvxAndroidColor _mvxNativeColor = new MvxAndroidColor();

        public static Android.Graphics.Color ToNativeColor(this MvxColor color)
        {
            return _mvxNativeColor.ToNativeColor(color);
        }
    }
}