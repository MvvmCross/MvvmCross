// MvxTouchLocalFileImageLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Threading.Tasks;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Plugins.File;
using UIKit;

namespace Cirrious.MvvmCross.Plugins.DownloadCache.Touch
{
	public class MvxTouchLocalFileImageLoader
        : MvxAllThreadDispatchingObject
        , IMvxLocalFileImageLoader<UIImage>
	{
		private const string ResourcePrefix = "res:";

		public Task<MvxImage<UIImage>> Load(string localPath, bool shouldCache, int width, int height)
		{
            var tcs = new TaskCompletionSource<MvxImage<UIImage>>();

            InvokeOnMainThread(() => {
                UIImage uiImage;

                if (localPath.StartsWith(ResourcePrefix))
                    uiImage = LoadResourceImage(localPath.Substring(ResourcePrefix.Length));
                else
                    uiImage = LoadUiImage(localPath);

                var result = (MvxImage<UIImage>)new MvxTouchImage(uiImage);

                tcs.TrySetResult(result);
            });

		    return tcs.Task;
		}

		private static UIImage LoadUiImage(string localPath)
		{
			var file = Mvx.Resolve<IMvxFileStore>();
			var nativePath = file.NativePath(localPath);
			return UIImage.FromFile(nativePath);
		}

		private static UIImage LoadResourceImage(string resourcePath)
		{
			return UIImage.FromBundle(resourcePath);
		}
    }
}