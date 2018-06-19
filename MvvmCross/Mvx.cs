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
        /// <summary>
        /// Returns a singleton instance of the default IoC Provider. If possible use dependency injection instead.
        /// </summary>
        public static IMvxIoCProvider IoCProvider => MvxSingleton<IMvxIoCProvider>.Instance;

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static bool CanResolve<TService>() where TService : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.CanResolve<TService>();
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static bool CanResolve(Type serviceType)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.CanResolve(serviceType);
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static TService Resolve<TService>() where TService : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.Resolve<TService>();
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static object Resolve(Type serviceType)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.Resolve(serviceType);
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
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

        [Obsolete("Use Mvx.IoCProvider instead")]
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

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static T Create<T>()
            where T : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.Create<T>();
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static object Create(Type type)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.Create(type);
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static T GetSingleton<T>()
            where T : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.GetSingleton<T>();
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static object GetSingleton(Type type)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.GetSingleton(type);
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static void RegisterType<TType>()
            where TType : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.RegisterType<TType>();
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static void RegisterType<TInterface, TType>()
            where TInterface : class
            where TType : class, TInterface
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.RegisterType<TInterface, TType>();
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static void RegisterType<TInterface>(Func<TInterface> constructor)
            where TInterface : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.RegisterType(constructor);
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static void RegisterType(Type tType)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.RegisterType(tType);
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static void RegisterType(Type type, Func<object> constructor)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.RegisterType(type, constructor);
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static void RegisterType(Type tInterface, Type tType)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.RegisterType(tInterface, tType);
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static void RegisterSingleton<TInterface>(Func<TInterface> serviceConstructor)
            where TInterface : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.RegisterSingleton(serviceConstructor);
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static void RegisterSingleton(Type tInterface, Func<object> serviceConstructor)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.RegisterSingleton(tInterface, serviceConstructor);
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static void RegisterSingleton<TInterface>(TInterface service)
            where TInterface : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.RegisterSingleton(service);
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static void RegisterSingleton(Type tInterface, object service)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.RegisterSingleton(tInterface, service);
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static T IoCConstruct<T>()
            where T : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.IoCConstruct<T>();
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static T IoCConstruct<T>(IDictionary<string, object> arguments)
            where T : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.IoCConstruct<T>(arguments);
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static T IoCConstruct<T>(object arguments)
            where T : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.IoCConstruct<T>(arguments);
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static T IoCConstruct<T>(params object[] arguments)
            where T : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.IoCConstruct<T>(arguments);
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static object IoCConstruct(Type type)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.IoCConstruct(type);
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static object IoCConstruct(Type type, IDictionary<string, object> arguments)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.IoCConstruct(type, arguments);
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static object IoCConstruct(Type type, object arguments)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.IoCConstruct(type, arguments);
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static object IoCConstruct(Type type, params object[] arguments)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.IoCConstruct(type, arguments);
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static void CallbackWhenRegistered<T>(Action<T> action)
            where T : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.CallbackWhenRegistered<T>(action);
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static void CallbackWhenRegistered<T>(Action action)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.CallbackWhenRegistered<T>(action);
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static void CallbackWhenRegistered(Type type, Action action)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.CallbackWhenRegistered(type, action);
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static void ConstructAndRegisterSingleton<TInterface, TType>()
            where TInterface : class
            where TType : class, TInterface
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.ConstructAndRegisterSingleton<TInterface, TType>();
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static void LazyConstructAndRegisterSingleton<TInterface, TType>()
            where TInterface : class
            where TType : class, TInterface
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.LazyConstructAndRegisterSingleton<TInterface, TType>();
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static void LazyConstructAndRegisterSingleton<TInterface>(Func<TInterface> constructor)
            where TInterface : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.LazyConstructAndRegisterSingleton<TInterface>(constructor);
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static void LazyConstructAndRegisterSingleton(Type type, Func<object> constructor)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.LazyConstructAndRegisterSingleton(type, constructor);
        }

        [Obsolete("Use Mvx.IoCProvider instead")]
        public static IMvxIoCProvider CreateChildContainer()
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.CreateChildContainer();
        }
    }
}
