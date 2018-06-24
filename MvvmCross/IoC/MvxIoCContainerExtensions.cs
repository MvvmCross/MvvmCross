using System;
using System.Collections.Generic;
using System.Text;

namespace MvvmCross.IoC
{
    public static class MvxIoCContainerExtensions
    {
        public static void CallbackWhenRegistered<T>(this IMvxIoCProvider ioc, Action<T> action)
            where T : class
        {
            Action simpleAction = () =>
            {
                var t = ioc.Resolve<T>();
                action(t);
            };
            ioc.CallbackWhenRegistered<T>(simpleAction);
        }

        public static TType ConstructAndRegisterSingleton<TInterface, TType>(this IMvxIoCProvider ioc)
            where TInterface : class
            where TType : class, TInterface
        {
            var instance = ioc.IoCConstruct<TType>();
            ioc.RegisterSingleton<TInterface>(instance);
            return instance;
        }

        public static TType ConstructAndRegisterSingleton<TInterface, TType>(this IMvxIoCProvider ioc, IDictionary<string, object> arguments)
            where TInterface : class
            where TType : class, TInterface
        {
            var instance = ioc.IoCConstruct<TType>(arguments);
            ioc.RegisterSingleton<TInterface>(instance);
            return instance;
        }

        public static TType ConstructAndRegisterSingleton<TInterface, TType>(this IMvxIoCProvider ioc, object arguments)
            where TInterface : class
            where TType : class, TInterface
        {
            var instance = ioc.IoCConstruct<TType>(arguments);
            ioc.RegisterSingleton<TInterface>(instance);
            return instance;
        }

        public static TType ConstructAndRegisterSingleton<TInterface, TType>(this IMvxIoCProvider ioc, params object[] arguments)
            where TInterface : class
            where TType : class, TInterface
        {
            var instance = ioc.IoCConstruct<TType>(arguments);
            ioc.RegisterSingleton<TInterface>(instance);
            return instance;
        }

        public static object ConstructAndRegisterSingleton(this IMvxIoCProvider ioc, Type type)
        {
            var instance = ioc.IoCConstruct(type);
            ioc.RegisterSingleton(type, instance);
            return instance;
        }

        public static object ConstructAndRegisterSingleton(this IMvxIoCProvider ioc, Type type, IDictionary<string, object> arguments)
        {
            var instance = ioc.IoCConstruct(type, arguments);
            ioc.RegisterSingleton(type, instance);
            return instance;
        }

        public static object ConstructAndRegisterSingleton(this IMvxIoCProvider ioc, Type type, object arguments)
        {
            var instance = ioc.IoCConstruct(type, arguments);
            ioc.RegisterSingleton(type, instance);
            return instance;
        }

        public static object ConstructAndRegisterSingleton(this IMvxIoCProvider ioc, Type type, params object[] arguments)
        {
            var instance = ioc.IoCConstruct(type, arguments);
            ioc.RegisterSingleton(type, instance);
            return instance;
        }

        public static void LazyConstructAndRegisterSingleton<TInterface, TType>(this IMvxIoCProvider ioc)
            where TInterface : class
            where TType : class, TInterface
        {
            ioc.RegisterSingleton<TInterface>(() => ioc.IoCConstruct<TType>());
        }

        public static void LazyConstructAndRegisterSingleton<TInterface>(this IMvxIoCProvider ioc, Func<TInterface> constructor)
            where TInterface : class
        {
            ioc.RegisterSingleton<TInterface>(constructor);
        }

        public static void LazyConstructAndRegisterSingleton(this IMvxIoCProvider ioc, Type type, Func<object> constructor)
        {
            ioc.RegisterSingleton(type, constructor);
        }

        public static void RegisterType<TType>(this IMvxIoCProvider ioc)
            where TType : class
        {
            ioc.RegisterType<TType, TType>();
        }

        public static void RegisterType(this IMvxIoCProvider ioc, Type tType)
        {
            ioc.RegisterType(tType, tType);
        }
    }
}
