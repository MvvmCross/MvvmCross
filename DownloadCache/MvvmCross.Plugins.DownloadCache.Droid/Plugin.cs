// Plugin.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Graphics;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.Plugins;

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
            Mvx.RegisterSingleton<IMvxImageCache<Bitmap>>(() => CreateCache());
            Mvx.RegisterType<IMvxImageHelper<Bitmap>, MvxDynamicImageHelper<Bitmap>>();
            Mvx.RegisterSingleton<IMvxLocalFileImageLoader<Bitmap>>(() => new MvxAndroidLocalFileImageLoader());
        }

        private MvxHttpFileDownloader CreateHttpFileDownloader()
        {
            var configuration = _configuration ?? MvxDownloadCacheConfiguration.Default;
            return new MvxHttpFileDownloader(configuration.MaxConcurrentDownloads);
        }

        private MvxImageCache<Bitmap> CreateCache()
        {
            var configuration = _configuration ?? MvxDownloadCacheConfiguration.Default;

            var fileDownloadCache = new MvxFileDownloadCache(configuration.CacheName,
                                                             configuration.CacheFolderPath,
                                                             configuration.MaxFiles,
                                                             configuration.MaxFileAge);
            var fileCache = new MvxImageCache<Bitmap>(fileDownloadCache, configuration.MaxInMemoryFiles, configuration.MaxInMemoryBytes, configuration.DisposeOnRemoveFromCache);
            return fileCache;
        }
    }
}