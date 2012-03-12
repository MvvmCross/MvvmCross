#region Copyright
// <copyright file="MvxImageRequest.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Images;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Platform.Images
{
    public class MvxImageRequest<T>
        : IMvxServiceConsumer<IMvxImageCache<T>>
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

        public event EventHandler<MvxExceptionEventArgs> Error;
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
                                           handler(this, new MvxExceptionEventArgs(exception));
                                   });
        }
    }
}