// MvxImageRequest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Platform;

namespace Cirrious.MvvmCross.Plugins.DownloadCache
{
    public class MvxImageRequest<T>
        : IMvxServiceConsumer
    {
        private readonly string _url;

        public MvxImageRequest(string url)
        {
            _url = url;
        }

        public string Url
        {
            get { return _url; }
        }

        public event EventHandler<MvxValueEventArgs<Exception>> Error;
        public event EventHandler<MvxValueEventArgs<T>> Complete;

        public void Start()
        {
			var cache = this.GetService<IMvxImageCache<T>>();
            cache.RequestImage(_url,
                               (image) =>
                                   {
                                       var handler = Complete;
                                       if (handler != null)
                                           handler(this, new MvxValueEventArgs<T>(image));
                                   },
                               (exception) =>
                                   {
                                       var handler = Error;
                                       if (handler != null)
                                           handler(this, new MvxValueEventArgs<Exception>(exception));
                                   });
        }
    }
}