// MvxServiceProvider.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
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
            return _iocProvider.TryGetService(out service);
        }

        public virtual void RegisterServiceType<TInterface, TToConstruct>()
            where TInterface : class
            where TToConstruct : class, TInterface
        {
#if DEBUG
            if (_iocProvider == null)
                throw new MvxException("IoC provider not set");
#endif
            _iocProvider.RegisterServiceType<TInterface, TToConstruct>();
        }

        public virtual void RegisterServiceInstance<TInterface>(TInterface theObject)
            where TInterface : class
        {
#if DEBUG
            if (_iocProvider == null)
                throw new MvxException("IoC provider not set");
#endif
            _iocProvider.RegisterServiceInstance(theObject);
        }

        public virtual void RegisterServiceInstance<TInterface>(Func<TInterface> theConstructor)
            where TInterface : class
        {
#if DEBUG
            if (_iocProvider == null)
                throw new MvxException("IoC provider not set");
#endif
            _iocProvider.RegisterServiceInstance(theConstructor);
        }
    }
}