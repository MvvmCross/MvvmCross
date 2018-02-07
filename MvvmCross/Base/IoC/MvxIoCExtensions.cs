﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Base.IoC
{
    [Obsolete("We prefer to use IoC directly using Mvx.Resolve<T>() now")]
    public static class MvxIoCExtensions
    {
        public static bool IsServiceAvailable<TService>(this IMvxConsumer consumer) where TService : class
        {
            return Mvx.CanResolve<TService>();
        }

        public static TService GetService<TService>(this IMvxConsumer consumer) where TService : class
        {
            return Mvx.Resolve<TService>();
        }

        public static bool TryGetService<TService>(this IMvxConsumer consumer, out TService service)
            where TService : class
        {
            return Mvx.TryResolve(out service);
        }

        public static void RegisterServiceInstance<TInterface>(this IMvxProducer producer,
                                                               Func<TInterface> serviceConstructor)
            where TInterface : class
        {
            Mvx.RegisterSingleton(serviceConstructor);
        }

        public static void RegisterServiceInstance<TInterface>(this IMvxProducer producer,
                                                               TInterface service)
            where TInterface : class
        {
            Mvx.RegisterSingleton(service);
        }

        public static void RegisterServiceType<TInterface, TType>(this IMvxProducer producer)
            where TInterface : class
            where TType : class, TInterface
        {
            Mvx.RegisterType<TInterface, TType>();
        }
    }
}
