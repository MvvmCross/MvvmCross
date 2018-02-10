// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Base;
using MvvmCross.Exceptions;
using MvvmCross.Plugins;
using UIKit;

namespace MvvmCross.Plugin.DownloadCache.Platform.iOS
{
#warning One day I would like to decouple this plugin from the FileStore plugin

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

        public void Load()
        {
            Mvx.RegisterSingleton<IMvxHttpFileDownloader>(CreateHttpFileDownloader);

            Mvx.RegisterSingleton<IMvxFileDownloadCache>(CreateFileDownloadCache);
            Mvx.RegisterSingleton<IMvxImageCache<UIImage>>(CreateCache);
            Mvx.RegisterType<IMvxImageHelper<UIImage>, MvxDynamicImageHelper<UIImage>>();
            Mvx.RegisterSingleton<IMvxLocalFileImageLoader<UIImage>>(() => new MvxIosLocalFileImageLoader());
        }

        private MvxHttpFileDownloader CreateHttpFileDownloader()
        {
            var configuration = _configuration ?? MvxDownloadCacheConfiguration.Default;
            return new MvxHttpFileDownloader(configuration.MaxConcurrentDownloads);
        }

        private MvxImageCache<UIImage> CreateCache()
        {
            var configuration = _configuration ?? MvxDownloadCacheConfiguration.Default;
            var fileDownloadCache = Mvx.Resolve<IMvxFileDownloadCache>();
            var fileCache = new MvxImageCache<UIImage>(fileDownloadCache, configuration.MaxInMemoryFiles, configuration.MaxInMemoryBytes, configuration.DisposeOnRemoveFromCache);
            return fileCache;
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
    }
}
