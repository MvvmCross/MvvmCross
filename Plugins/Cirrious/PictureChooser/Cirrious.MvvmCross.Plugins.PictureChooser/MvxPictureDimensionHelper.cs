// MvxPictureDimensionHelper.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.MvvmCross.Plugins.PictureChooser
{
    public static class MvxPictureDimensionHelper
    {
        public static void TargetWidthAndHeight(int maxPixelDimension, int currentWidth, int currentHeight, out int targetWidth, out int targetHeight)
        {
            var ratio = 1.0;
            if (currentWidth > currentHeight)
            {
                var maxPixel = (currentWidth > maxPixelDimension) ? maxPixelDimension : currentWidth;
                ratio = (maxPixel) / ((double)currentWidth);
            }
            else
            {
                var maxPixel = (currentHeight > maxPixelDimension) ? maxPixelDimension : currentHeight;
                ratio = (maxPixel) / ((double)currentHeight);
            }

            targetWidth = (int)Math.Round(ratio * currentWidth);
            targetHeight = (int)Math.Round(ratio * currentHeight);
        }
    }
}