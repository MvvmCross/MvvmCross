// Plugin.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.Plugins;
using UIKit;

namespace MvvmCross.Plugins.DownloadCache.Touch
{
#warning One day I would like to decouple this plugin from the FileStore plugin
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
            Mvx.RegisterSingleton<IMvxImageCache<UIImage>>(CreateCache);
            Mvx.RegisterType<IMvxImageHelper<UIImage>, MvxDynamicImageHelper<UIImage>>();
            Mvx.RegisterSingleton<IMvxLocalFileImageLoader<UIImage>>(() => new MvxTouchLocalFileImageLoader());
        }

        private MvxHttpFileDownloader CreateHttpFileDownloader()
        {
            var configuration = _configuration ?? MvxDownloadCacheConfiguration.Default;
            return new MvxHttpFileDownloader(configuration.MaxConcurrentDownloads);
        }

        private MvxImageCache<UIImage> CreateCache()
        {
            var configuration = _configuration ?? MvxDownloadCacheConfiguration.Default;

            var fileDownloadCache = new MvxFileDownloadCache(configuration.CacheName,
                                                             configuration.CacheFolderPath,
                                                             configuration.MaxFiles,
                                                             configuration.MaxFileAge);
            var fileCache = new MvxImageCache<UIImage>(fileDownloadCache, configuration.MaxInMemoryFiles, configuration.MaxInMemoryBytes, configuration.DisposeOnRemoveFromCache);
            return fileCache;
        }
    }
}