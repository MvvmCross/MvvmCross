using System;
using Cirrious.CrossCore.Plugins;

namespace Cirrious.MvvmCross.Plugins.DownloadCache.WindowsPhone
{
    public class MvxDownloadCacheConfiguration
        : IMvxPluginConfiguration
    {
        public static readonly MvxDownloadCacheConfiguration Default = new MvxDownloadCacheConfiguration();

        public string CacheName { get; set; }
        public string CacheFolderPath { get; set; }
        public int MaxFiles { get; set; }
        public TimeSpan MaxFileAge { get; set; }
        public int MaxInMemoryFiles { get; set; }
        public int MaxInMemoryBytes { get; set; }

        public MvxDownloadCacheConfiguration()
        {
            CacheName = "Pictures.MvvmCross";
            CacheFolderPath = "";
            MaxFiles = 500;
            MaxFileAge = TimeSpan.FromDays(7);
            MaxInMemoryBytes = 4000000; // 4 MB
            MaxInMemoryFiles = 30;
        }
    }
}