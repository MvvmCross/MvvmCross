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
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Plugins.DownloadCache.Touch
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
            var fileDownloadCache = new MvxFileDownloadCache("Pictures.MvvmCross",
                                                             "../Library/Caches/Pictures.MvvmCross/", 500,
                                                             TimeSpan.FromDays(3.0));
            var fileCache = new MvxImageCache<UIImage>(fileDownloadCache, 30, 4000000);
            this.RegisterServiceInstance<IMvxImageCache<UIImage>>(fileCache);

            this.RegisterServiceInstance<IMvxLocalFileImageLoader<UIImage>>(new MvxTouchLocalFileImageLoader());
        }

        #endregion
    }
}