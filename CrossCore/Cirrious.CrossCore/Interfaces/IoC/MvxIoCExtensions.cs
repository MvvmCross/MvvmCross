// MvxServiceProviderExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Core;

namespace Cirrious.CrossCore.Interfaces.IoC
{
    public static class MvxIoCExtensions
    {
        public static bool CanResolve<TService>() where TService : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.CanResolve<TService>();
        }

        public static bool CanResolve<TService>(this IMvxConsumer consumer) where TService : class
        {
            return CanResolve<TService>();
        }

        public static TService Resolve<TService>(this IMvxConsumer consumer) where TService : class
        {
            return Resolve<TService>();
        }

        public static TService Resolve<TService>() where TService : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.Resolve<TService>();
        }

        public static bool TryResolve<TService>(this IMvxConsumer consumer, out TService service)
            where TService : class
        {
            return TryResolve(out service);
        }

        public static bool TryResolve<TService>(out TService service) where TService : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.TryResolve(out service);
        }

        public static void RegisterSingleton<TInterface>(this IMvxProducer producer,
                                                               Func<TInterface> serviceConstructor)
            where TInterface : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.RegisterSingleton(serviceConstructor);
        }

        public static void RegisterSingleton<TInterface>(this IMvxProducer producer,
                                                               TInterface service)
            where TInterface : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.RegisterSingleton(service);
        }

        public static void RegisterType<TInterface, TType>(this IMvxProducer producer)
            where TInterface : class
            where TType : class, TInterface
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.RegisterType<TInterface, TType>();
        }
    }
}