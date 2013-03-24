// MvxTouchLocalFileImageLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.Plugins.File;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Plugins.DownloadCache.Touch
{
    public class MvxTouchLocalFileImageLoader
        : IMvxLocalFileImageLoader<UIImage>    
    {
		private const string ResourcePrefix = "res:";

        public MvxImage<UIImage> Load(string localPath, bool shouldCache)
        {
			UIImage uiImage;
            if (localPath.StartsWith(ResourcePrefix))
			{
				var resourcePath = localPath.Substring(ResourcePrefix.Length);
				uiImage = LoadResourceImage(resourcePath, shouldCache);
			}
			else
			{
				uiImage = LoadUIImage(localPath);
			}
            return new MvxTouchImage(uiImage);
        }

        private UIImage LoadUIImage(string localPath)
        {
            var file = Mvx.Resolve<IMvxFileStore>();
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
        }

		private UIImage LoadResourceImage(string resourcePath, bool shouldCache)
		{
			if (shouldCache)
				return UIImage.FromFile(resourcePath);
			else
				return UIImage.FromFileUncached(resourcePath);
		}
    }
}