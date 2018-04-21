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

        public static void ConstructAndRegisterSingleton<TInterface, TType>(this IMvxIoCProvider ioc)
            where TInterface : class
            where TType : class, TInterface
        {
            ioc.RegisterSingleton<TInterface>(ioc.IoCConstruct<TType>());
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
