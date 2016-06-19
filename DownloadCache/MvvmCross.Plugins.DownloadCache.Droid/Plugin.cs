// Plugin.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Graphics;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.Plugins;

namespace MvvmCross.Plugins.DownloadCache.Droid
{
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

            var fileDownloadCache = CreateFileDownloadCache();

            Mvx.RegisterSingleton<IMvxFileDownloadCache>(fileDownloadCache);
            Mvx.RegisterSingleton<IMvxImageCache<Bitmap>>(() => CreateCache(fileDownloadCache));
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

        private MvxImageCache<Bitmap> CreateCache(IMvxFileDownloadCache fileDownloadCache)
        {
            var configuration = _configuration ?? MvxDownloadCacheConfiguration.Default;


            var fileCache = new MvxImageCache<Bitmap>(fileDownloadCache, configuration.MaxInMemoryFiles, configuration.MaxInMemoryBytes, configuration.DisposeOnRemoveFromCache);
            return fileCache;
        }
    }
}