#region Copyright
// <copyright file="UIImageHelpers.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Drawing;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Plugins.PictureChooser.Touch
{
    public static class UIImageHelpers
    {
        public static UIImage FromFile(string filename, SizeF fitSize)
        {
            var imageFile = UIImage.FromFile(filename);

            return imageFile.ImageToFitSize(fitSize);
        }

        public static UIImage ImageToFitSize(this UIImage image, SizeF fitSize)
        {
            var imageScaleFactor = 1.0;
            imageScaleFactor = image.CurrentScale;

            var sourceWidth = image.Size.Width*imageScaleFactor;
            var sourceHeight = image.Size.Height*imageScaleFactor;
            var targetWidth = fitSize.Width;
            var targetHeight = fitSize.Height;

            var sourceRatio = sourceWidth/sourceHeight;
            var targetRatio = targetWidth/targetHeight;

            var scaleWidth = (sourceRatio <= targetRatio);
            scaleWidth = !scaleWidth;

            double scalingFactor;
            double scaledWidth;
            double scaledHeight;

            if (scaleWidth)
            {
                scalingFactor = 1.0/sourceRatio;
                scaledWidth = targetWidth;
                scaledHeight = Math.Round(targetWidth*scalingFactor);
            }
            else
            {
                scalingFactor = sourceRatio;
                scaledWidth = Math.Round(targetHeight*scalingFactor);
                scaledHeight = targetHeight;
            }

            var destRect = new RectangleF(0, 0, (float) scaledWidth, (float) scaledHeight);

            UIGraphics.BeginImageContextWithOptions(destRect.Size, false, 0.0f);
            image.Draw(destRect);

            var newImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return newImage;
        }
    }
}