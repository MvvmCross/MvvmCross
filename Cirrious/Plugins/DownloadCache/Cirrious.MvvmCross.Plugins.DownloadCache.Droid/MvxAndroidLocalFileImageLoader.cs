// MvxAndroidLocalFileImageLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Graphics;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Plugins.File;

namespace Cirrious.MvvmCross.Plugins.DownloadCache.Droid
{
    public class MvxAndroidLocalFileImageLoader
        : IMvxLocalFileImageLoader<Bitmap>
          , IMvxServiceConsumer
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