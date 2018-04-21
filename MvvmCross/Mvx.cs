// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MvvmCross.Base;
using MvvmCross.IoC;

namespace MvvmCross
{
    public static class Mvx
    {
        public static bool CanResolve<TService>() where TService : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.CanResolve<TService>();
        }

        public static bool CanResolve(Type serviceType)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.CanResolve(serviceType);
        }

        public static TService Resolve<TService>() where TService : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.Resolve<TService>();
        }

        public static object Resolve(Type serviceType)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.Resolve(serviceType);
        }

        public static bool TryResolve<TService>(out TService service) where TService : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            if (ioc == null)
            {
                service = null;
                return false;
            }
            return ioc.TryResolve(out service);
        }

        public static bool TryResolve(Type serviceType, out object service)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            if (ioc == null)
            {
                service = null;
                return false;
            }
            return ioc.TryResolve(serviceType, out service);
        }

        public static T Create<T>()
            where T : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.Create<T>();
        }

        public static object Create(Type type)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.Create(type);
        }

        public static T GetSingleton<T>()
            where T : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.GetSingleton<T>();
        }

        public static object GetSingleton(Type type)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.GetSingleton(type);
        }

        public static void RegisterType<TType>()
            where TType : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.RegisterType<TType>();
        }

        public static void RegisterType<TInterface, TType>()
            where TInterface : class
            where TType : class, TInterface
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.RegisterType<TInterface, TType>();
        }

        public static void RegisterType<TInterface>(Func<TInterface> constructor)
            where TInterface : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.RegisterType(constructor);
        }

        public static void RegisterType(Type tType)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.RegisterType(tType);
        }

        public static void RegisterType(Type type, Func<object> constructor)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.RegisterType(type, constructor);
        }

        public static void RegisterType(Type tInterface, Type tType)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.RegisterType(tInterface, tType);
        }

        public static void RegisterSingleton<TInterface>(Func<TInterface> serviceConstructor)
            where TInterface : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.RegisterSingleton(serviceConstructor);
        }

        public static void RegisterSingleton(Type tInterface, Func<object> serviceConstructor)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.RegisterSingleton(tInterface, serviceConstructor);
        }

        public static void RegisterSingleton<TInterface>(TInterface service)
            where TInterface : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.RegisterSingleton(service);
        }

        public static void RegisterSingleton(Type tInterface, object service)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.RegisterSingleton(tInterface, service);
        }

        public static T IoCConstruct<T>()
            where T : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.IoCConstruct<T>();
        }

        public static T IoCConstruct<T>(IDictionary<string, object> arguments)
            where T : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.IoCConstruct<T>(arguments);
        }

        public static T IoCConstruct<T>(object arguments)
            where T : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.IoCConstruct<T>(arguments);
        }

        public static T IoCConstruct<T>(params object[] arguments)
            where T : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.IoCConstruct<T>(arguments);
        }

        public static object IoCConstruct(Type type)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.IoCConstruct(type);
        }

        public static object IoCConstruct(Type type, IDictionary<string, object> arguments)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.IoCConstruct(type, arguments);
        }

        public static object IoCConstruct(Type type, object arguments)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.IoCConstruct(type, arguments);
        }

        public static object IoCConstruct(Type type, params object[] arguments)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.IoCConstruct(type, arguments);
        }

        public static void CallbackWhenRegistered<T>(Action<T> action)
            where T : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.CallbackWhenRegistered<T>(action);
        }

        public static void CallbackWhenRegistered<T>(Action action)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.CallbackWhenRegistered<T>(action);
        }

        public static void CallbackWhenRegistered(Type type, Action action)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.CallbackWhenRegistered(type, action);
        }

        public static void ConstructAndRegisterSingleton<TInterface, TType>()
            where TInterface : class
            where TType : class, TInterface
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.ConstructAndRegisterSingleton<TInterface, TType>();
        }

        public static void LazyConstructAndRegisterSingleton<TInterface, TType>()
            where TInterface : class
            where TType : class, TInterface
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.LazyConstructAndRegisterSingleton<TInterface, TType>();
        }

        public static void LazyConstructAndRegisterSingleton<TInterface>(Func<TInterface> constructor)
            where TInterface : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.LazyConstructAndRegisterSingleton<TInterface>(constructor);
        }

        public static void LazyConstructAndRegisterSingleton(Type type, Func<object> constructor)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.LazyConstructAndRegisterSingleton(type, constructor);
        }

        public static IMvxIoCProvider CreateChildContainer()
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.CreateChildContainer();
        }
    }
}
