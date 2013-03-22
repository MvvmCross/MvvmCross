// Plugin.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Graphics;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.Plugins;

namespace Cirrious.MvvmCross.Plugins.DownloadCache.Droid
{
    public class Plugin
        : IMvxPlugin          
    {
        public void Load()
        {
#warning One day I would like to decouple this implementation from the FileStore plugin
			File.PluginLoader.Instance.EnsureLoaded();

            Mvx.RegisterSingleton<IMvxHttpFileDownloader>(new MvxHttpFileDownloader());

#warning Huge Magic numbers here - what cache sizes should be used?
            try
            {
                var fileDownloadCache = new MvxFileDownloadCache("_PicturesMvvmCross", "_Caches/Pictures.MvvmCross/",
                                                                 500, TimeSpan.FromDays(3.0));
                var fileCache = new MvxImageCache<Bitmap>(fileDownloadCache, 30, 4000000);
                Mvx.RegisterSingleton<IMvxImageCache<Bitmap>>(fileCache);

                Mvx.RegisterType<IMvxImageHelper<Bitmap>, MvxDynamicImageHelper<Bitmap>>();
                Mvx.RegisterSingleton<IMvxLocalFileImageLoader<Bitmap>>(new MvxAndroidLocalFileImageLoader());
            }
            catch (Exception exception)
            {
                MvxTrace.Error( "Binding", "Exception {0}", exception.ToLongString());
                throw;
            }
        }
    }
}