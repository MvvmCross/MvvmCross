﻿// MvxIoCExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.CrossCore.Interfaces.IoC
{
    [Obsolete("We prefer to use IoC directly using Mvx.Resolve<T>() now")]
    public static class MvxIoCExtensions
    {
        public static bool CanResolve<TService>(this IMvxConsumer consumer) where TService : class
        {
            return Mvx.CanResolve<TService>();
        }

        public static TService Resolve<TService>(this IMvxConsumer consumer) where TService : class
        {
            return Mvx.Resolve<TService>();
        }

        public static bool TryResolve<TService>(this IMvxConsumer consumer, out TService service)
            where TService : class
        {
            return Mvx.TryResolve(out service);
        }

        public static void RegisterSingleton<TInterface>(this IMvxProducer producer,
                                                         Func<TInterface> serviceConstructor)
            where TInterface : class
        {
            Mvx.RegisterSingleton(serviceConstructor);
        }

        public static void RegisterSingleton<TInterface>(this IMvxProducer producer,
                                                         TInterface service)
            where TInterface : class
        {
            Mvx.RegisterSingleton(service);
        }

        public static void RegisterType<TInterface, TType>(this IMvxProducer producer)
            where TInterface : class
            where TType : class, TInterface
        {
            Mvx.RegisterType<TInterface, TType>();
        }
    }
}