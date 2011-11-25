using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace MonoTouch.NinePatch
{
	public enum ResizeMethod
	{
		Stretch,
		Tile
	}
	
	public class NinePatchImage
	{
		// create an empty NinePatch
		public NinePatchImage()
		{
			_resizeMethod = ResizeMethod.Tile;
		}
		
		// create a NinePatch from file
		public NinePatchImage(string fileName, ResizeMethod resizeMethod)
		{
			_resizeMethod = resizeMethod;
			
			const uint BLACK_PIXEL_FULL_ALPHA = 0x000000FF;
			
			// base image
			UIImage ninePatchIamge = UIImage.FromFile(fileName);
			
			// get lower level representaton so we can test pixels for blackness
			CGImage dataImage = ninePatchIamge.CGImage;
			IntPtr dataPointer = Marshal.AllocHGlobal(dataImage.Width * dataImage.Height * 4);

			CGBitmapContext context = new CGBitmapContext(dataPointer, dataImage.Width, dataImage.Height,
				dataImage.BitsPerComponent, dataImage.BytesPerRow, dataImage.ColorSpace,
			    CGImageAlphaInfo.PremultipliedFirst);

			context.SetFillColorWithColor(UIColor.White.CGColor);
			context.FillRect(new RectangleF(0, 0, dataImage.Width, dataImage.Height));
			context.DrawImage(new RectangleF(0, 0, dataImage.Width, dataImage.Height), dataImage);

			int topStart = 0;
			int topEnd = dataImage.Width;
			int leftStart = 0;
			int leftEnd = dataImage.Height;
			int bottomStart = 0;
			int bottomEnd = dataImage.Width;
			int rightStart = 0;
			int rightEnd = dataImage.Height;
			
			bool noPaddingSpecified = false;
			int centerHeight = 0;
			int centerWidth = 0;
			
			unsafe
			{
				// calculate top stretch line
				int firstPixel = dataImage.Width;
				int lastPixel = 0;
				uint* imagePointer = (uint*) (void*) dataPointer;
				for (int xx = 0; xx < dataImage.Width; xx++, imagePointer++)
				{
					uint thisValue = *imagePointer;
					if (*imagePointer == BLACK_PIXEL_FULL_ALPHA)
					{
						if (xx < firstPixel)
							firstPixel = xx;
						if (xx > lastPixel)
							lastPixel = xx;
					}
				}
				topStart = firstPixel;
				topEnd = lastPixel;
				_leftWidth = topStart - 1;
				_rightWidth = (dataImage.Width - 2) - topEnd; // assumes padding lines (-1 if not)!
				centerWidth = (dataImage.Width - 2) - (_leftWidth + _rightWidth);
				
				// calculate left side stretch line
				firstPixel = dataImage.Height;
				lastPixel = 0;
				imagePointer = (uint*) (void*) dataPointer;
				for (int xx = 0; xx < dataImage.Height; xx++, imagePointer += dataImage.Width)
				{
					uint thisValue = *imagePointer;
					if (thisValue == BLACK_PIXEL_FULL_ALPHA)
					{
						if (xx < firstPixel)
							firstPixel = xx;
						if (xx > lastPixel)
							lastPixel = xx;
					}
				}
				leftStart = firstPixel;
				leftEnd = lastPixel;
				_upperHeight = leftStart - 1;
				_lowerHeight = (dataImage.Height - 2) - leftEnd;
				centerHeight = (dataImage.Height - 2) - (_upperHeight + _lowerHeight);
				
				// calculate right side padding line
				firstPixel = dataImage.Height;
				lastPixel = 0;
				imagePointer = ((uint*) (void*) dataPointer) + (dataImage.Width - 1);
				for (int xx = 0; xx < dataImage.Height; xx++, imagePointer += dataImage.Width)
				{
					uint thisValue = *imagePointer;
					if (thisValue == BLACK_PIXEL_FULL_ALPHA)
					{
						if (xx < firstPixel)
							firstPixel = xx;
						if (xx > lastPixel)
							lastPixel = xx;
					}
				}
				if (lastPixel == 0)
				{
					noPaddingSpecified = true;
				}
				rightStart = firstPixel;
				rightEnd = lastPixel;
				
				// calculate bottom padding line
				firstPixel = dataImage.Width;
				lastPixel = 0;
				imagePointer = ((uint*) (void*) dataPointer) + (dataImage.Width * (dataImage.Height - 1));
				for (int xx = 0; xx < dataImage.Width; xx++, imagePointer++)
				{
					uint thisValue = *imagePointer;
					if (*imagePointer == BLACK_PIXEL_FULL_ALPHA)
					{
						if (xx < firstPixel)
							firstPixel = xx;
						if (xx > lastPixel)
							lastPixel = xx;
					}
				}
				if (lastPixel == 0)
				{
					noPaddingSpecified = true;
				}
				bottomStart = firstPixel;
				bottomEnd = lastPixel;				
			}
			Marshal.FreeHGlobal(dataPointer);
			
			// upper section bitmaps
			Rectangle imageRect = new Rectangle(1, 1, _leftWidth, _upperHeight);
			//Debug.WriteLine("Upper Left: " + imageRect.ToString());
			_upperLeft = IsRect(imageRect) ? UIImage.FromImage(dataImage.WithImageInRect(imageRect)): null;

			imageRect = new Rectangle(topStart, 1, centerWidth, _upperHeight);
			//Debug.WriteLine("Upper: " + imageRect.ToString());
			_upper = IsRect(imageRect) ? UIImage.FromImage(dataImage.WithImageInRect(imageRect)) : null;
			
			imageRect = new Rectangle(topEnd + 1, 1, _rightWidth, _upperHeight);
			//Debug.WriteLine("Upper Right: " + imageRect.ToString());
			_upperRight = IsRect(imageRect) ? UIImage.FromImage(dataImage.WithImageInRect(imageRect)) : null;
		
			// center section bitmaps
			imageRect = new Rectangle(1, leftStart, _leftWidth, centerHeight);
			//Debug.WriteLine("Left: " + imageRect.ToString());
			_left = IsRect(imageRect) ? UIImage.FromImage(dataImage.WithImageInRect(imageRect)) : null; 

			imageRect = new Rectangle(topStart, leftStart, centerWidth, centerHeight);
			//Debug.WriteLine("Center: " + imageRect.ToString());
			_center = IsRect(imageRect) ? UIImage.FromImage(dataImage.WithImageInRect(imageRect)) : null;

			imageRect = new Rectangle(topEnd + 1, leftStart, _rightWidth, centerHeight);
			//Debug.WriteLine("Right: " + imageRect.ToString());
			_right = IsRect(imageRect) ? UIImage.FromImage(dataImage.WithImageInRect(imageRect)) : null;

			// lower section bitmaps
			imageRect = new Rectangle(1, leftEnd + 1, _leftWidth, _lowerHeight);
			//Debug.WriteLine("Lower Left: " + imageRect.ToString());
			_lowerLeft = IsRect(imageRect) ? UIImage.FromImage(dataImage.WithImageInRect(imageRect)) : null;
			
			imageRect = new Rectangle(topStart, leftEnd + 1, centerWidth, _lowerHeight);
			//Debug.WriteLine("Lower: " + imageRect.ToString());
			_lower = IsRect(imageRect) ? UIImage.FromImage(dataImage.WithImageInRect(imageRect)) : null;
			
			imageRect = new Rectangle(topEnd + 1, leftEnd + 1, _rightWidth, _lowerHeight);
			//Debug.WriteLine("Lower Right: " + imageRect.ToString());
			_lowerRight = IsRect(imageRect) ? UIImage.FromImage(dataImage.WithImageInRect(imageRect)) : null;
			
			if (noPaddingSpecified)
				// uses the center area defined by the top and left margins
				_paddingBounds = new Rectangle(_upperLeft.CGImage.Width, _upperLeft.CGImage.Height, _center.CGImage.Width, _center.CGImage.Height);
			else
				// uses the area defined by the bottom and right margins
				_paddingBounds = new Rectangle(bottomStart - 1, rightStart - 1, bottomEnd - bottomStart, rightEnd - rightStart);
			
			//Debug.WriteLine("Content/Padding: " + _paddingBounds.ToString());
		}

		#region Internal Class Members
		private int _upperHeight;
		private int _lowerHeight;
		private int _leftWidth;
		private int _rightWidth;
		
		private UIImage _upperLeft;
		private UIImage _upper;
		private UIImage _upperRight;
		private UIImage _right;
		private UIImage _lowerRight;
		private UIImage _lower;
		private UIImage _lowerLeft;
		private UIImage _left;
		private UIImage _center;
		private Rectangle _paddingBounds;
		private ResizeMethod _resizeMethod;
		
		private static bool IsRect(RectangleF rect)
		{
			return (rect.Height > 0 && rect.Width > 0);
		}
		#endregion
		
		public UIImage UpperLeft { get { return _upperLeft; } }
		public UIImage Upper { get { return _upper; } }
		public UIImage UpperRight { get { return _upperRight; } }
		public UIImage Left { get { return _left; } }
		public UIImage Center { get { return _center; } }
		public UIImage Right { get { return _right; } }
		public UIImage Lower { get { return _lower; } }
		public UIImage LowerLeft { get { return _lowerLeft; } }
		public UIImage LowerRight { get { return _lowerRight; } }

		/// <summary>
		/// Retrieves the height of the source .png file (before resizing).
		/// </summary>
		public int IntrinsicHeight { get { return _upperHeight + _center.CGImage.Height + _lowerHeight; } }
		
		/// <summary>
		/// Retrieves the width of the source .png file (before resizing).
		/// </summary>
		public int IntrinsicWidth { get { return _leftWidth + _center.CGImage.Width + _rightWidth; } }

		/// <summary>
		/// Returns the minimum height suggested by this Drawable.
		/// </summary>
		public int MinimumHeight { get { return _upperHeight + _lowerHeight; } }

		/// <summary>
		/// Returns the minimum width suggested by this Drawable.
		/// </summary>
		public int MinimumWidth { get { return _leftWidth + _rightWidth; } }

		/// <summary>
		/// Returns the size/location of the content center relative to the top corner
		/// <//summary>
		public RectangleF ContentRectangle { get { return _paddingBounds; } }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="size">
		/// A <see cref="SizeF"/>
		/// </param>
		/// <returns>
		/// A <see cref="RectangleF"/>
		/// </returns>
		public RectangleF GetContentRectangleForSize(SizeF size)
		{
			// start with the current padding rect
			RectangleF content = this.ContentRectangle;
			
			// add to it the size increase
			content.Height += size.Height - this.IntrinsicHeight;
			content.Width += size.Width - this.IntrinsicWidth;
			return content;
		}
		
		/// <summary>
		/// Draw the stretched/sized nine patch into the device context,
		/// used where we don't want the overhead of creating a large bitmap, but instead paint the background
		/// </summary>
		/// <param name="rect">
		/// A <see cref="RectangleF"/>
		/// </param>
		/// <param name="resizeMethod">
		/// A <see cref="ResizeMethod"/>
		/// </param>
		public void Draw(RectangleF rect)
		{
			SizeF size = rect.Size;
			int offsetX = (int) rect.Left;
			int offsetY = (int) rect.Top;
			
			// validate that requested size is greater than the original image
			int centerWidth = (int) (size.Width - (_leftWidth + _rightWidth));
			int centerHeight = (int) (size.Height - (_upperHeight + _lowerHeight));

			if (centerWidth <= 0 || centerHeight <= 0)
			{
				throw new ArgumentException("Specified size is too small for specified nine-patch image.");
			}
			
			// draw each image in the appropriate rectangle
			if (_upperLeft != null)
				_upperLeft.Draw(new Point(offsetX, offsetY));
			if (_upper != null) 
				Draw(_upper, new RectangleF(offsetX + _leftWidth, offsetY, centerWidth, _upper.CGImage.Height));
			if (_upperRight != null)
				_upperRight.Draw(new PointF(offsetX + _leftWidth + centerWidth, offsetY));

			if (_left != null)
				Draw(_left, new RectangleF(offsetX, offsetY + _upperHeight, _left.CGImage.Width, centerHeight));
			if (_center != null)
				Draw(_center, new RectangleF(offsetX + _leftWidth, offsetY + _upperHeight, centerWidth, centerHeight));
			if (_right != null)
				Draw(_right, new RectangleF(offsetX + _leftWidth + centerWidth, offsetY + _upperHeight, _right.CGImage.Width, centerHeight));

			if (_lowerLeft != null)
				_lowerLeft.Draw(new PointF(offsetX, offsetY + _upperHeight + centerHeight));
			if (_lower != null)
				Draw(_lower, new RectangleF(offsetX + _leftWidth, offsetY + _upperHeight + centerHeight, centerWidth, _lower.CGImage.Height));
			if (_lowerRight != null)
				_lowerRight.Draw(new PointF(offsetX + _leftWidth + centerWidth, offsetY + _upperHeight + centerHeight));
		}
		
		private static float min(float x1, float x2) { return (x1 < x2) ? x1: x2; }	
		
		//
		// Can't draw tiles yourself as they will always be stretched if the destination rectangle isn't the same as the
		// image rectangle...
		//
		// to get around Apple iOS SDK bug (DrawAsPatternInRect() is broken)
		//
		private void Draw(UIImage image, RectangleF fillRect)
		{
			if (_resizeMethod == ResizeMethod.Stretch)
			{
				image.Draw(fillRect);
			}
			else 
			{
				// should be:
				// tile.DrawAsPatternInRect(fillRect)...
			
				Size tileSize = new Size(image.CGImage.Width, image.CGImage.Height);
				for (float xx = fillRect.Left; xx < fillRect.Left + fillRect.Width; xx += tileSize.Width)
				{
					for (float yy = fillRect.Top; yy < fillRect.Top + fillRect.Height; yy += tileSize.Height)				
					{
						float height = min(tileSize.Height, fillRect.Top + fillRect.Height - yy);
						float width = min(tileSize.Width, fillRect.Left + fillRect.Width - xx);
	
						RectangleF rectPaint = new RectangleF(xx, yy, width, height);
						if (width > 0 && height > 0)
							image.Draw(rectPaint);
					}
				}
			}
		}
		
		/// <summary>
		/// Get a background image that spans the given target size
		/// </summary>
		/// <param name="size">
		/// A <see cref="SizeF"/>
		/// </param>
		/// <param name="resizeMethod">
		/// A <see cref="ResizeMethod"/>
		/// </param>
		/// <returns>
		/// A <see cref="UIImage"/>
		/// </returns>
		public UIImage GetImageForSize(SizeF size)
		{
			// start a context for the resulting image
			UIGraphics.BeginImageContext(size);
			
			RectangleF rect = new RectangleF(new Point(0, 0), size);
			
			Draw(rect);
			
			// Get the image
			UIImage combinedImage = UIGraphics.GetImageFromCurrentImageContext();

			// End the image context
			UIGraphics.EndImageContext();
			
			return combinedImage;
		}
	}
}
