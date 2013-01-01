// MvxTouchImage.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Plugins.DownloadCache.Touch
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
            return cg.BytesPerRow*cg.Height;
        }
    }
}