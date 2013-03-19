// MvxAndroidLocalFileImageLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Graphics;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.Plugins.File;

namespace Cirrious.MvvmCross.Plugins.DownloadCache.Droid
{
    public class MvxAndroidLocalFileImageLoader
        : IMvxLocalFileImageLoader<Bitmap>
          
    {
        #region IMvxLocalFileImageLoader<UIImage> Members

        public MvxImage<Bitmap> Load(string localPath, bool shouldCache)
        {
            var bitmap = LoadBitmap(localPath, shouldCache);
            return new MvxAndroidImage(bitmap);
        }

        private Bitmap LoadBitmap(string localPath, bool shouldCache)
        {
            var fileStore = Mvx.Resolve<IMvxFileStore>();
            byte[] contents;
            if (!fileStore.TryReadBinaryFile(localPath, out contents))
                return null;

			// the InPurgeable option is very important for Droid memory management.
			// see http://slodge.blogspot.co.uk/2013/02/huge-android-memory-bug-and-bug-hunting.html
			var options = new BitmapFactory.Options () { InPurgeable = true };
            var image = BitmapFactory.DecodeByteArray(contents, 0, contents.Length, options);
            return image;
        }

        #endregion
    }
}