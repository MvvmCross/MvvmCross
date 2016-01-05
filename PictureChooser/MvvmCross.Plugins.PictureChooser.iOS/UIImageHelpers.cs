// UIImageHelpers.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using CoreGraphics;
using UIKit;

namespace MvvmCross.Plugins.PictureChooser.iOS
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

            var scaleWidth = (sourceRatio <= targetRatio);
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

            UIGraphics.BeginImageContextWithOptions(destRect.Size, false, 0);
            image.Draw(destRect);

            var newImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return newImage;
        }
    }
}