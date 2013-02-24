// MvxServiceProviderExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Platform;

namespace Cirrious.CrossCore.Interfaces.ServiceProvider
{
    public static class MvxIoCExtensions
    {
        public static bool IsServiceAvailable<TService>() where TService : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.CanResolve<TService>();
        }

        public static bool IsServiceAvailable<TService>(this IMvxConsumer consumer) where TService : class
        {
            return IsServiceAvailable<TService>();
        }

        public static TService GetService<TService>(this IMvxConsumer consumer) where TService : class
        {
            return GetService<TService>();
        }

        public static TService GetService<TService>() where TService : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.Resolve<TService>();
        }

        public static bool TryGetService<TService>(this IMvxConsumer consumer, out TService service)
            where TService : class
        {
            return TryGetService(out service);
        }

        public static bool TryGetService<TService>(out TService service) where TService : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.TryResolve(out service);
        }

        public static void RegisterServiceInstance<TInterface>(this IMvxProducer producer,
                                                               Func<TInterface> serviceConstructor)
            where TInterface : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.RegisterSingleton(serviceConstructor);
        }

        public static void RegisterServiceInstance<TInterface>(this IMvxProducer producer,
                                                               TInterface service)
            where TInterface : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.RegisterSingleton(service);
        }

        public static void RegisterServiceType<TInterface, TType>(this IMvxProducer producer)
            where TInterface : class
            where TType : class, TInterface
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.RegisterType<TInterface, TType>();
        }
    }
}