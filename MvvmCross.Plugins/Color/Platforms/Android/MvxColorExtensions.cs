﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Plugin.Color.Platforms.Android
{
    public static class MvxColorExtensions
    {
        private static readonly MvxAndroidColor _mvxNativeColor = new MvxAndroidColor();

        public static global::Android.Graphics.Color ToNativeColor(this System.Drawing.Color color)
        {
            return _mvxNativeColor.ToNativeColor(color);
        }
    }
}
