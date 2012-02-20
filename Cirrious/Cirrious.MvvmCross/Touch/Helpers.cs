using System;
using System.Drawing;

namespace MonoTouch.UIKit.Extensions
{
	public static class UIImageHelper
	{
		public static UIImage FromFile (string filename, SizeF fitSize)
		{
			var imageFile = UIImage.FromFile (filename);
	 
			return imageFile.ImageToFitSize(fitSize);
		}
	 
		public static UIImage ImageToFitSize (this UIImage image, SizeF fitSize)
		{
	  	    double imageScaleFactor = 1.0;
		    imageScaleFactor = image.CurrentScale;
	 
		    double sourceWidth = image.Size.Width * imageScaleFactor;
		    double sourceHeight = image.Size.Height * imageScaleFactor;
		    double targetWidth = fitSize.Width;
		    double targetHeight = fitSize.Height;
	 
		    double sourceRatio = sourceWidth / sourceHeight;
		    double targetRatio = targetWidth / targetHeight;
	 
		    bool scaleWidth = (sourceRatio <= targetRatio);
		    scaleWidth = !scaleWidth;
	 
		    double scalingFactor, scaledWidth, scaledHeight;
	 
		    if (scaleWidth) 
		    {
		        scalingFactor = 1.0 / sourceRatio;
		        scaledWidth = targetWidth;
		        scaledHeight = Math.Round (targetWidth * scalingFactor);
		    } 
			else 
			{
		        scalingFactor = sourceRatio;
		        scaledWidth = Math.Round(targetHeight * scalingFactor);
				scaledHeight = targetHeight;
			}
	 
			RectangleF destRect = new RectangleF(0, 0, (float)scaledWidth, (float)scaledHeight);
	 
			UIGraphics.BeginImageContextWithOptions(destRect.Size, false, 0.0f);
			image.Draw (destRect); 
			
			UIImage newImage = UIGraphics.GetImageFromCurrentImageContext ();
			UIGraphics.EndImageContext ();
	 
			return newImage;
		}
	}
}

