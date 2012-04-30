using System;
using Android.Graphics;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Plugins.DownloadCache.Droid
{
    public class Plugin
        : IMvxPlugin
        , IMvxServiceProducer<IMvxLocalFileImageLoader<Bitmap>>
        , IMvxServiceProducer<IMvxImageCache<Bitmap>>
        , IMvxServiceProducer<IMvxHttpFileDownloader>
    {
        #region Implementation of IMvxPlugin

        public void Load()
        {
            File.PluginLoader.Instance.EnsureLoaded();

            this.RegisterServiceInstance<IMvxHttpFileDownloader>(new MvxHttpFileDownloader());

#warning Huge Magic numbers here
            var fileDownloadCache = new MvxFileDownloadCache("_PicturesMvvmCross", "_Caches/Pictures.MvvmCross/", 500, TimeSpan.FromDays(3.0));
            var fileCache = new MvxImageCache<Bitmap>(fileDownloadCache, 30, 4000000);
            this.RegisterServiceInstance<IMvxImageCache<Bitmap>>(fileCache);

            this.RegisterServiceInstance<IMvxLocalFileImageLoader<Bitmap>>(new MvxAndroidLocalFileImageLoader());
        }

        #endregion
    }
}