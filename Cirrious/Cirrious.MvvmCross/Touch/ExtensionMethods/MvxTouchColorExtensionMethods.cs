#region Copyright
// <copyright file="MvxTouchColorExtensionMethods.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Touch.ExtensionMethods
{
    public static class MvxTouchColorExtensionMethods 
	{
		public static UIColor ColorFromInt(this uint rgbValue)
		{
			float red = (rgbValue & 0xFF0000) >> 16;
			float green = (rgbValue & 0xFF00) >> 8;
			float blue = rgbValue & 0xFF;
			return UIColor.FromRGB(red/255, green/255, blue/255);
		}
		
		public static UIColor ColorWithAlphaFromInt(this uint rgbaValue)
		{
			float red = (rgbaValue & 0xFF0000) >> 16;
			float green = (rgbaValue & 0xFF00) >> 8;
			float blue = rgbaValue & 0xFF;
			float alpha = (rgbaValue & 0xFF000000) >> 24;
			return UIColor.FromRGBA(red/255, green/255, blue/255, alpha/255);
		}
		
		public static uint IntFromColor(this UIColor color)
		{
			float red, green, blue, alpha;
			color.GetRGBA(out red, out green, out blue, out alpha);
			uint rgbaValue = (uint) (((long) alpha) << 24 | ((long) red) << 16 | ((long) green) << 8 | ((long) blue));
			return rgbaValue;
		}
	}
}

