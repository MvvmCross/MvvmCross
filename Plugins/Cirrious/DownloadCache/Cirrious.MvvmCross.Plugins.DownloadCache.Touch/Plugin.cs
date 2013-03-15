// Plugin.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Interfaces.Platform;
using Cirrious.CrossCore.Interfaces.Plugins;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Plugins.DownloadCache.Touch
{
    public class Plugin
        : IMvxPlugin
    {
        public void Load()
        {
            File.PluginLoader.Instance.EnsureLoaded();

            Mvx.RegisterSingleton<IMvxHttpFileDownloader>(new MvxHttpFileDownloader());

#warning Huge Magic numbers here - what cache sizes should be used?
            var fileDownloadCache = new MvxFileDownloadCache("Pictures.MvvmCross",
                                                             "../Library/Caches/Pictures.MvvmCross/", 500,
                                                             TimeSpan.FromDays(3.0));
            var fileCache = new MvxImageCache<UIImage>(fileDownloadCache, 30, 4000000);
            Mvx.RegisterSingleton<IMvxImageCache<UIImage>>(fileCache);

            Mvx.RegisterType<IMvxImageHelper<UIImage>, MvxDynamicImageHelper<UIImage>>();
            Mvx.RegisterSingleton<IMvxLocalFileImageLoader<UIImage>>(new MvxTouchLocalFileImageLoader());
        }
    }
}