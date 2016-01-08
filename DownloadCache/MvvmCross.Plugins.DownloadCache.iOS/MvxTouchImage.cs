// MvxIosImage.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using UIKit;

namespace MvvmCross.Plugins.DownloadCache.iOS
{
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
            return (int)(cg.BytesPerRow * cg.Height);
        }
    }
}