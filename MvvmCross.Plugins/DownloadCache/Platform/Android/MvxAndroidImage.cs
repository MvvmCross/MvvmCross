// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Graphics;
using Android.Support.V4.Graphics;

namespace MvvmCross.Plugin.DownloadCache.Platform.Android
{
    [Preserve(AllMembers = true)]
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
