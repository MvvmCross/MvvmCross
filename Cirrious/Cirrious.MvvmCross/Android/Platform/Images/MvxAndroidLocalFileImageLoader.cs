#region Copyright
// <copyright file="MvxAndroidLocalFileImageLoader.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Android.Graphics;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Platform.Images;

namespace Cirrious.MvvmCross.Android.Platform.Images
{
    public class MvxAndroidLocalFileImageLoader
        : IMvxLocalFileImageLoader<Bitmap>
        , IMvxServiceConsumer<IMvxSimpleFileStoreService>
    {
        #region IMvxLocalFileImageLoader<UIImage> Members

        public MvxImage<Bitmap> Load(string localPath, bool shouldCache)
        {
            var bitmap = LoadBitmap(localPath, shouldCache);
            return new MvxAndroidImage(bitmap);
        }

        private Bitmap LoadBitmap(string localPath, bool shouldCache)
        {
            var fileStore = this.GetService<IMvxSimpleFileStoreService>();
            byte[] contents;
            if (!fileStore.TryReadBinaryFile(localPath, out contents))
                return null;

            var image = BitmapFactory.DecodeByteArray(contents, 0, contents.Length);
            return image;
        }

        #endregion
    }
}