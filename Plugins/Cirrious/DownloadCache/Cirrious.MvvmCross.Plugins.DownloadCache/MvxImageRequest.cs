// MvxImageRequest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Interfaces.Core;
using Cirrious.CrossCore.Interfaces.IoC;

namespace Cirrious.MvvmCross.Plugins.DownloadCache
{
    public class MvxImageRequest<T>
        : IMvxConsumer
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
            var weakThis = new WeakReference(this);
            cache.RequestImage(_url,
                               (image) =>
                                   {
                                       var strongThis = (MvxImageRequest<T>) weakThis.Target;
                                       if (strongThis == null)
                                           return;

                                       var handler = strongThis.Complete;
                                       if (handler != null)
                                           handler(this, new MvxValueEventArgs<T>(image));
                                   },
                               (exception) =>
                                   {
                                       var strongThis = (MvxImageRequest<T>) weakThis.Target;
                                       if (strongThis == null)
                                           return;

                                       var handler = strongThis.Error;
                                       if (handler != null)
                                           handler(this, new MvxValueEventArgs<Exception>(exception));
                                   });
        }
    }
}