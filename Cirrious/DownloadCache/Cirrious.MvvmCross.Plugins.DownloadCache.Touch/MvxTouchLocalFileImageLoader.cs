// MvxTouchLocalFileImageLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Plugins.File;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Plugins.DownloadCache.Touch
{
    public class MvxTouchLocalFileImageLoader
        : IMvxLocalFileImageLoader<UIImage>
          , IMvxServiceConsumer
    {
        #region IMvxLocalFileImageLoader<UIImage> Members

        public MvxImage<UIImage> Load(string localPath, bool shouldCache)
        {
            // shouldCache ignored
            var uiImage = LoadUIImage(localPath);
            return new MvxTouchImage(uiImage);
        }

        private UIImage LoadUIImage(string localPath)
        {
            var file = this.GetService<IMvxFileStore>();
            byte[] data = null;
            if (!file.TryReadBinaryFile(localPath, stream =>
                {
                    var memoryStream = new System.IO.MemoryStream();
                    stream.CopyTo(memoryStream);
                    data = memoryStream.GetBuffer();
                    return true;
                }))
                return null;

            var imageData = NSData.FromArray(data);
            return UIImage.LoadFromData(imageData);

            //this code was never going to work?!
            //if (shouldCache)
            //    return UIImage.FromFile(localPath);s
            //else
            //    return UIImage.FromFileUncached(localPath);
        }

        #endregion
    }
}