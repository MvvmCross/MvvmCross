// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using UIKit;
using ObjCRuntime;

namespace MvvmCross.Platforms.Ios
{
    public static class MvxIosColorExtensions
    {
        public static UIColor ColorFromInt(this uint rgbValue)
        {
            nfloat red = (rgbValue & 0xFF0000) >> 16;
            nfloat green = (rgbValue & 0xFF00) >> 8;
            nfloat blue = rgbValue & 0xFF;
            return UIColor.FromRGB(red / 255, green / 255, blue / 255);
        }

        public static UIColor ColorWithAlphaFromInt(this uint rgbaValue)
        {
            nfloat red = (rgbaValue & 0xFF0000) >> 16;
            nfloat green = (rgbaValue & 0xFF00) >> 8;
            nfloat blue = rgbaValue & 0xFF;
            nfloat alpha = (rgbaValue & 0xFF000000) >> 24;
            return UIColor.FromRGBA(red / 255, green / 255, blue / 255, alpha / 255);
        }

        public static uint IntFromColor(this UIColor color)
        {
            nfloat red, green, blue, alpha;
            color.GetRGBA(out red, out green, out blue, out alpha);
            var rgbaValue = (uint)(((long)alpha << 24) | ((long)red << 16) | ((long)green << 8) | (long)blue);
            return rgbaValue;
        }
    }
}
