// MvxTouchLocalFileImageLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Threading.Tasks;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.File;
using Foundation;
using UIKit;

namespace Cirrious.MvvmCross.Plugins.DownloadCache.Touch
{
	public class MvxTouchLocalFileImageLoader
        : IMvxLocalFileImageLoader<UIImage>
	{
		private const string ResourcePrefix = "res:";

		public Task<MvxImage<UIImage>> Load(string localPath, bool shouldCache, int width, int height)
		{
		    return Task.Run(() =>
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

                return (MvxImage<UIImage>)new MvxTouchImage(uiImage);
		    });
		}

		private UIImage LoadUIImage(string localPath)
		{
			var file = Mvx.Resolve<IMvxFileStore>();
			var nativePath = file.NativePath(localPath);
			return UIImage.FromFile(nativePath);
		}

		private UIImage LoadResourceImage(string resourcePath, bool shouldCache)
		{
			return UIImage.FromBundle(resourcePath);
		}
    }
}