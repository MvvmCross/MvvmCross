// Plugin.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Graphics;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Interfaces.Platform;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.CrossCore.Interfaces.Plugins;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.CrossCore.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Plugins.DownloadCache.Droid
{
    public class Plugin
        : IMvxPlugin
          , IMvxProducer
    {
        #region Implementation of IMvxPlugin

        public void Load()
        {
            File.PluginLoader.Instance.EnsureLoaded();

            this.RegisterServiceInstance<IMvxHttpFileDownloader>(new MvxHttpFileDownloader());

#warning Huge Magic numbers here - what cache sizes should be used?
            try
            {
                var fileDownloadCache = new MvxFileDownloadCache("_PicturesMvvmCross", "_Caches/Pictures.MvvmCross/",
                                                                 500, TimeSpan.FromDays(3.0));
                var fileCache = new MvxImageCache<Bitmap>(fileDownloadCache, 30, 4000000);
                this.RegisterServiceInstance<IMvxImageCache<Bitmap>>(fileCache);

                this.RegisterServiceType<IMvxImageHelper<Bitmap>, MvxDynamicImageHelper<Bitmap>>();
                this.RegisterServiceInstance<IMvxLocalFileImageLoader<Bitmap>>(new MvxAndroidLocalFileImageLoader());
            }
            catch (Exception exception)
            {
                MvxTrace.Trace(MvxTraceLevel.Error, "Binding", "Exception {0}", exception.ToLongString());
                throw;
            }
        }

        #endregion
    }
}