// Mvx.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;

namespace Cirrious.CrossCore
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

        public static T GetSingleton<T>()
            where T : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.GetSingleton<T>();
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

        public static void ConstructAndRegisterSingleton<TInterface, TType>()
            where TInterface : class
            where TType : TInterface
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.RegisterSingleton<TInterface>(IocConstruct<TType>());
        }

        public static void LazyConstructAndRegisterSingleton<TInterface, TType>()
            where TInterface : class
            where TType : TInterface
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.RegisterSingleton<TInterface>(() => IocConstruct<TType>());
        }

        public static void LazyConstructAndRegisterSingleton<TInterface>(Func<TInterface> constructor)
            where TInterface : class
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.RegisterSingleton<TInterface>(constructor);
        }

        public static void LazyConstructAndRegisterSingleton(Type type, Func<object> constructor)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            ioc.RegisterSingleton(type, constructor);
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

        public static T IocConstruct<T>()
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return (T) ioc.IoCConstruct(typeof (T));
        }

        public static object IocConstruct(Type t)
        {
            var ioc = MvxSingleton<IMvxIoCProvider>.Instance;
            return ioc.IoCConstruct(t);
        }

        public static void CallbackWhenRegistered<T>(Action<T> action)
            where T : class
        {
            Action simpleAction = () =>
                {
                    var t = Mvx.Resolve<T>();
                    action(t);
                };
            CallbackWhenRegistered<T>(simpleAction);
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

        public static void TaggedTrace(MvxTraceLevel level, string tag, string message, params object[] args)
        {
            MvxTrace.TaggedTrace(level, tag, message, args);
        }

        public static void Trace(MvxTraceLevel level, string message, params object[] args)
        {
            MvxTrace.Trace(level, message, args);
        }

        public static void TaggedTrace(string tag, string message, params object[] args)
        {
            TaggedTrace(MvxTraceLevel.Diagnostic, tag, message, args);
        }

        public static void TaggedWarning(string tag, string message, params object[] args)
        {
            TaggedTrace(MvxTraceLevel.Warning, tag, message, args);
        }

        public static void TaggedError(string tag, string message, params object[] args)
        {
            TaggedTrace(MvxTraceLevel.Error, tag, message, args);
        }

        public static void Trace(string message, params object[] args)
        {
            Trace(MvxTraceLevel.Diagnostic, message, args);
        }

        public static void Warning(string message, params object[] args)
        {
            Trace(MvxTraceLevel.Warning, message, args);
        }

        public static void Error(string message, params object[] args)
        {
            Trace(MvxTraceLevel.Error, message, args);
        }

        public static MvxException Exception(string message)
        {
            return new MvxException(message);
        }

        public static MvxException Exception(string message, params object[] args)
        {
            return new MvxException(message, args);
        }

        public static MvxException Exception(Exception innerException, string message, params object[] args)
        {
            return new MvxException(innerException, message, args);
        }
    }
}
