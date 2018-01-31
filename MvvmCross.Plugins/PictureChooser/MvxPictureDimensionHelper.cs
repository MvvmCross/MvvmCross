// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Plugins.PictureChooser
{
    public static class MvxPictureDimensionHelper
    {
        public static void TargetWidthAndHeight(int maxPixelDimension, int currentWidth, int currentHeight, out int targetWidth, out int targetHeight)
        {
            var ratio = 1.0;
            if (currentWidth > currentHeight)
                ratio = maxPixelDimension / (double)currentWidth;
            else
                ratio = maxPixelDimension / (double)currentHeight;

            targetWidth = (int)Math.Round(ratio * currentWidth);
            targetHeight = (int)Math.Round(ratio * currentHeight);
        }
    }
}