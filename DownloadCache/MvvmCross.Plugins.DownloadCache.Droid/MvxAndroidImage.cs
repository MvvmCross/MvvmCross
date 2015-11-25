// MvxAndroidImage.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Graphics;
using Android.Support.V4.Graphics;

namespace MvvmCross.Plugins.DownloadCache.Droid
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

            return BitmapCompat.GetAllocationByteCount(RawImage);
        }
    }
}