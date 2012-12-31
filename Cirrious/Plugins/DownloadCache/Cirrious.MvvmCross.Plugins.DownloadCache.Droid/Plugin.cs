#region Copyright

// <copyright file="Plugin.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using Android.Graphics;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Plugins.DownloadCache.Droid
{
    public class Plugin
        : IMvxPlugin
          , IMvxServiceProducer
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