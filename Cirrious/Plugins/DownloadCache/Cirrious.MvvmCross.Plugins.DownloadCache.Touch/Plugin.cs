using System;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Plugins.DownloadCache.Touch
{
    public class Plugin
        : IMvxPlugin
        , IMvxServiceProducer<IMvxImageCache<UIImage>>
        , IMvxServiceProducer<IMvxLocalFileImageLoader<UIImage>>
        , IMvxServiceProducer<IMvxHttpFileDownloader>
    {
        #region Implementation of IMvxPlugin

        public void Load()
        {
            File.PluginLoader.Instance.EnsureLoaded();

            this.RegisterServiceInstance<IMvxHttpFileDownloader>(new MvxHttpFileDownloader());

#warning Huge Magic numbers here
            var fileDownloadCache = new MvxFileDownloadCache("Pictures.MvvmCross", "../Library/Caches/Pictures.MvvmCross/", 500, TimeSpan.FromDays(3.0));
            var fileCache = new MvxImageCache<UIImage>(fileDownloadCache, 30, 4000000);
            this.RegisterServiceInstance<IMvxImageCache<UIImage>>(fileCache);

            this.RegisterServiceInstance<IMvxLocalFileImageLoader<UIImage>>(new MvxTouchLocalFileImageLoader());
        }

        #endregion
    }
}