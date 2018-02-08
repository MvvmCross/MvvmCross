// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Base;
using UIKit;

namespace MvvmCross.Plugin.DownloadCache.Platform.iOS
{
    [Preserve(AllMembers = true)]
	public class MvxIosImage
        : MvxImage<UIImage>
    {
        public MvxIosImage(UIImage rawImage)
            : base(rawImage)
        {
        }

        public override int GetSizeInBytes()
        {
            if (RawImage == null)
                return 0;

            var cg = RawImage.CGImage;
            if (cg == null)
                return 0;

            return (int)(cg.BytesPerRow * cg.Height);
        }
    }
}
