// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Graphics;
using MvvmCross.Base;
using MvvmCross.Base.Exceptions;
using MvvmCross.Base.Platform;
using MvvmCross.Base.Plugins;

namespace MvvmCross.Plugin.DownloadCache.Platform.Android
{
    [Preserve(AllMembers = true)]
    public class Plugin
        : IMvxConfigurablePlugin
    {
        private MvxDownloadCacheConfiguration _configuration;

        public void Configure(IMvxPluginConfiguration configuration)
        {
            if (configuration != null && !(configuration is MvxDownloadCacheConfiguration))
            {
                throw new MvxException("You must use a MvxDownloadCacheConfiguration object for configuring the DownloadCache, but you supplied {0}", configuration.GetType().Name);
            }
            _configuration = (MvxDownloadCacheConfiguration)configuration;
        }

#warning One day I would like to decouple this implementation from the FileStore plugin

        public void Load()
        {
            Mvx.RegisterSingleton<IMvxHttpFileDownloader>(() => CreateHttpFileDownloader());

            Mvx.RegisterSingleton<IMvxFileDownloadCache>(CreateFileDownloadCache);
            Mvx.RegisterSingleton<IMvxImageCache<Bitmap>>(CreateCache);
            Mvx.RegisterType<IMvxImageHelper<Bitmap>, MvxDynamicImageHelper<Bitmap>>();
            Mvx.RegisterSingleton<IMvxLocalFileImageLoader<Bitmap>>(() => new MvxAndroidLocalFileImageLoader());
        }

        private MvxHttpFileDownloader CreateHttpFileDownloader()
        {
            var configuration = _configuration ?? MvxDownloadCacheConfiguration.Default;
            return new MvxHttpFileDownloader(configuration.MaxConcurrentDownloads);
        }

        private IMvxFileDownloadCache CreateFileDownloadCache()
        {
            var configuration = _configuration ?? MvxDownloadCacheConfiguration.Default;
            var fileDownloadCache = new MvxFileDownloadCache(configuration.CacheName,
                                                             configuration.CacheFolderPath,
                                                             configuration.MaxFiles,
                                                             configuration.MaxFileAge);

            return fileDownloadCache;
        }

        private MvxImageCache<Bitmap> CreateCache()
        {
            var configuration = _configuration ?? MvxDownloadCacheConfiguration.Default;

            var fileDownloadCache = Mvx.Resolve<IMvxFileDownloadCache>();
            var fileCache = new MvxImageCache<Bitmap>(fileDownloadCache, configuration.MaxInMemoryFiles, configuration.MaxInMemoryBytes, configuration.DisposeOnRemoveFromCache);
            return fileCache;
        }
    }
}
