#region Copyright
// <copyright file="MvxTouchImage.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Platform.Images;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Touch.Platform.Images
{
    public class MvxTouchImage
        : MvxImage<UIImage>
    {
        public MvxTouchImage(UIImage rawImage) 
            : base(rawImage)
        {
        }

        public override int GetSizeInBytes()
        {
            if (RawImage == null)
                return 0;

            var cg = RawImage.CGImage;
            return cg.BytesPerRow * cg.Height;
        }
    }
}