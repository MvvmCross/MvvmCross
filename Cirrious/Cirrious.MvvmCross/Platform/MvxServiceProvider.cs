#region Copyright
// <copyright file="MvxServiceProvider.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Core;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.Interfaces.IoC;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Platform
{
    public class MvxServiceProvider 
        : MvxSingleton<IMvxServiceProviderRegistry>
        , IMvxServiceProviderRegistry
    {
        private readonly IMvxIoCProvider _iocProvider;

        public MvxServiceProvider(IMvxIoCProvider iocProvider)
        {
            _iocProvider = iocProvider;
        }

        public virtual bool SupportsService<T>() where T : class
        {
#if DEBUG
            if (_iocProvider == null)
                throw new MvxException("IoC provider not set");
#endif
            return _iocProvider.SupportsService<T>();
        }

        public virtual T GetService<T>() where T : class
        {
#if DEBUG
            if (_iocProvider == null)
                throw new MvxException("IoC provider not set");
#endif
            return _iocProvider.GetService<T>();
        }

        public bool TryGetService<T>(out T service) where T : class
        {
#if DEBUG
            if (_iocProvider == null)
                throw new MvxException("IoC provider not set");
#endif
            return _iocProvider.TryGetService<T>(out service);
        }

        public virtual void RegisterServiceType<TFrom, TTo>()
        {
#if DEBUG
            if (_iocProvider == null)
                throw new MvxException("IoC provider not set");
#endif
            _iocProvider.RegisterServiceType<TFrom, TTo>();
        }

        public virtual void RegisterServiceInstance<TInterface>(TInterface theObject)
        {
#if DEBUG
            if (_iocProvider == null)
                throw new MvxException("IoC provider not set");
#endif
            _iocProvider.RegisterServiceInstance(theObject);
        }
    }
}