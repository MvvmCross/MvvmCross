// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using CoreGraphics;
using UIKit;

namespace MvvmCross.Plugin.PictureChooser.Platforms.Ios
{
    public static class UIImageHelpers
    {
        public static UIImage FromFile(string filename, CGSize fitSize)
        {
            var imageFile = UIImage.FromFile(filename);

            return imageFile.ImageToFitSize(fitSize);
        }

        public static UIImage ImageToFitSize(this UIImage image, CGSize fitSize)
        {
            var imageScaleFactor = 1.0;
            imageScaleFactor = image.CurrentScale;

            var sourceWidth = image.Size.Width * imageScaleFactor;
            var sourceHeight = image.Size.Height * imageScaleFactor;
            var targetWidth = fitSize.Width;
            var targetHeight = fitSize.Height;

            var sourceRatio = sourceWidth / sourceHeight;
            var targetRatio = targetWidth / targetHeight;

            var scaleWidth = sourceRatio <= targetRatio;
            scaleWidth = !scaleWidth;

            double scalingFactor;
            double scaledWidth;
            double scaledHeight;

            if (scaleWidth)
            {
                scalingFactor = 1.0 / sourceRatio;
                scaledWidth = targetWidth;
                scaledHeight = Math.Round(targetWidth * scalingFactor);
            }
            else
            {
                scalingFactor = sourceRatio;
                scaledWidth = Math.Round(targetHeight * scalingFactor);
                scaledHeight = targetHeight;
            }

            var destRect = new CGRect(0, 0, (nfloat)scaledWidth, (nfloat)scaledHeight);

            UIGraphics.BeginImageContextWithOptions(destRect.Size, false, 1);
            image.Draw(destRect);

            var newImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return newImage;
        }
    }
}
