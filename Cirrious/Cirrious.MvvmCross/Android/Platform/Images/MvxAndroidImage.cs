#region Copyright
// <copyright file="MvxAndroidImage.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Android.Graphics;
using Cirrious.MvvmCross.Platform.Images;

namespace Cirrious.MvvmCross.Android.Platform.Images
{
    public class MvxAndroidImage
        : MvxImage<Bitmap>
    {
        public MvxAndroidImage(Bitmap rawImage) 
            : base(rawImage)
        {
        }

        public override int GetSizeInBytes()
        {
            if (RawImage == null)
                return 0;

            return RawImage.RowBytes*RawImage.Height;
        }
    }
}