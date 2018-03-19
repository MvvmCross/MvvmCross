// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using MvvmCross.Base;
using MvvmCross.Plugin.File;
using UIKit;

namespace MvvmCross.Plugin.DownloadCache.Platform.Ios
{
    [Preserve(AllMembers = true)]
	public class MvxIosLocalFileImageLoader
        : MvxMainThreadDispatchingObject
        , IMvxLocalFileImageLoader<UIImage>
    {
        private const string ResourcePrefix = "res:";

        public Task<MvxImage<UIImage>> Load(string localPath, bool shouldCache, int width, int height)
        {
            var tcs = new TaskCompletionSource<MvxImage<UIImage>>();

            InvokeOnMainThread(() =>
            {
                UIImage uiImage;

                uiImage = localPath.StartsWith(ResourcePrefix) ? LoadResourceImage(localPath.Substring(ResourcePrefix.Length)) : LoadUiImage(localPath);

                var result = (MvxImage<UIImage>)new MvxIosImage(uiImage);

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
