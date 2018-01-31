// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace MvvmCross.Plugins.DownloadCache.Droid
{
    [Preserve(AllMembers = true)]
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
