// Plugin.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt

using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.Plugins;
using System.IO;

namespace Cirrious.MvvmCross.Plugins.DownloadCache.WindowsPhone
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

        public void Load()
        {
            Mvx.RegisterSingleton<IMvxHttpFileDownloader>(() => new MvxHttpFileDownloader());
            Mvx.RegisterSingleton<IMvxImageCache<byte[]>>(CreateCache);
            Mvx.RegisterType<IMvxImageHelper<byte[]>, MvxDynamicImageHelper<byte[]>>();
            Mvx.RegisterSingleton<IMvxLocalFileImageLoader<byte[]>>(() => new MvxWindowsPhoneLocalFileImageLoader());
        }

        private MvxImageCache<byte[]> CreateCache()
        {
            var configuration = _configuration ?? MvxDownloadCacheConfiguration.Default;

            var fileDownloadCache = new MvxFileDownloadCache(configuration.CacheName,
                                                             configuration.CacheFolderPath,
                                                             configuration.MaxFiles,
                                                             configuration.MaxFileAge);
            var fileCache = new MvxImageCache<byte[]>(fileDownloadCache, configuration.MaxInMemoryFiles, configuration.MaxInMemoryBytes);
            return fileCache;
        }
    }
}