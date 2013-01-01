// MvxTouchLocalFileImageLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Plugins.DownloadCache.Touch
{
    public class MvxTouchLocalFileImageLoader
        : IMvxLocalFileImageLoader<UIImage>
    {
        #region IMvxLocalFileImageLoader<UIImage> Members

        public MvxImage<UIImage> Load(string localPath, bool shouldCache)
        {
            var uiImage = LoadUIImage(localPath, shouldCache);
            return new MvxTouchImage(uiImage);
        }

        private static UIImage LoadUIImage(string localPath, bool shouldCache)
        {
            if (shouldCache)
                return UIImage.FromFile(localPath);
            else
                return UIImage.FromFileUncached(localPath);
        }

        #endregion
    }
}