// MvxDownloadCacheConfiguration.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Plugins;
using System;

namespace MvvmCross.Plugins.DownloadCache.Droid
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
        public int MaxConcurrentDownloads { get; set; }
        public bool DisposeOnRemoveFromCache { get; set; }

        public MvxDownloadCacheConfiguration()
        {
            CacheName = "_PicturesMvvmCross";
            CacheFolderPath = "_Caches/Pictures.MvvmCross/";
            MaxFiles = 500;
            MaxFileAge = TimeSpan.FromDays(7);
            MaxInMemoryBytes = 4000000; // 4 MB
            MaxInMemoryFiles = 30;
            MaxConcurrentDownloads = 10;
            DisposeOnRemoveFromCache = true;
        }
    }
}