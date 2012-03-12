#region Copyright
// <copyright file="MvxTouchLocalFileImageLoader.cs" company="Cirrious">
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