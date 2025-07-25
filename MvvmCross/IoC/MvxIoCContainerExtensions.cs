using System.Diagnostics.CodeAnalysis;

namespace MvvmCross.IoC
{
    public static class MvxIoCContainerExtensions
    {
        private static Func<TInterface> CreateResolver<
            TInterface,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter1>(
                this IMvxIoCProvider ioc, Func<TParameter1, TInterface> typedConstructor)
                    where TInterface : class
                    where TParameter1 : class
        {
            return () =>
            {
                ioc.TryResolve(typeof(TParameter1), out var parameter1);
                return typedConstructor((TParameter1)parameter1);
            };
        }

        private static Func<TInterface> CreateResolver<
            TInterface,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter1,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter2>(
                this IMvxIoCProvider ioc, Func<TParameter1, TParameter2, TInterface> typedConstructor)
                    where TInterface : class
                    where TParameter1 : class
                    where TParameter2 : class
        {
            return () =>
            {
                ioc.TryResolve(typeof(TParameter1), out var parameter1);
                ioc.TryResolve(typeof(TParameter2), out var parameter2);
                return typedConstructor((TParameter1)parameter1, (TParameter2)parameter2);
            };
        }

        private static Func<TInterface> CreateResolver<
            TInterface,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter1,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter2,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter3>(
                this IMvxIoCProvider ioc,
                Func<TParameter1, TParameter2, TParameter3, TInterface> typedConstructor)
                    where TInterface : class
                    where TParameter1 : class
                    where TParameter2 : class
                    where TParameter3 : class
        {
            return () =>
            {
                ioc.TryResolve(typeof(TParameter1), out var parameter1);
                ioc.TryResolve(typeof(TParameter2), out var parameter2);
                ioc.TryResolve(typeof(TParameter3), out var parameter3);
                return typedConstructor((TParameter1)parameter1, (TParameter2)parameter2, (TParameter3)parameter3);
            };
        }

        private static Func<TInterface> CreateResolver<
            TInterface,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter1,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter2,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter3,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter4>(
                this IMvxIoCProvider ioc,
                Func<TParameter1, TParameter2, TParameter3, TParameter4, TInterface> typedConstructor)
                    where TInterface : class
                    where TParameter1 : class
                    where TParameter2 : class
                    where TParameter3 : class
                    where TParameter4 : class
        {
            return () =>
            {
                ioc.TryResolve(typeof(TParameter1), out var parameter1);
                ioc.TryResolve(typeof(TParameter2), out var parameter2);
                ioc.TryResolve(typeof(TParameter3), out var parameter3);
                ioc.TryResolve(typeof(TParameter4), out var parameter4);
                return typedConstructor((TParameter1)parameter1, (TParameter2)parameter2, (TParameter3)parameter3, (TParameter4)parameter4);
            };
        }

        private static Func<TInterface> CreateResolver<
            TInterface,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter1,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter2,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter3,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter4,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter5>(
                this IMvxIoCProvider ioc,
                Func<TParameter1, TParameter2, TParameter3, TParameter4, TParameter5, TInterface> typedConstructor)
                    where TInterface : class
                    where TParameter1 : class
                    where TParameter2 : class
                    where TParameter3 : class
                    where TParameter4 : class
                    where TParameter5 : class
        {
            return () =>
            {
                ioc.TryResolve(typeof(TParameter1), out var parameter1);
                ioc.TryResolve(typeof(TParameter2), out var parameter2);
                ioc.TryResolve(typeof(TParameter3), out var parameter3);
                ioc.TryResolve(typeof(TParameter4), out var parameter4);
                ioc.TryResolve(typeof(TParameter5), out var parameter5);
                return typedConstructor((TParameter1)parameter1, (TParameter2)parameter2, (TParameter3)parameter3, (TParameter4)parameter4, (TParameter5)parameter5);
            };
        }

        public static TType ConstructAndRegisterSingleton<
            TInterface,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TType>(
                this IMvxIoCProvider ioc)
                    where TInterface : class
                    where TType : class, TInterface
        {
            var instance = ioc.IoCConstruct<TType>();
            ioc.RegisterSingleton<TInterface>(instance);
            return instance;
        }

        public static TType ConstructAndRegisterSingleton<
            TInterface,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TType>(
                this IMvxIoCProvider ioc, IDictionary<string, object> arguments)
                    where TInterface : class
                    where TType : class, TInterface
        {
            var instance = ioc.IoCConstruct<TType>(arguments);
            ioc.RegisterSingleton<TInterface>(instance);
            return instance;
        }

        public static TType ConstructAndRegisterSingleton<
            TInterface,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TType>(
                this IMvxIoCProvider ioc,
                object arguments)
                    where TInterface : class
                    where TType : class, TInterface
        {
            var instance = ioc.IoCConstruct<TType>(arguments);
            ioc.RegisterSingleton<TInterface>(instance);
            return instance;
        }

        public static TType ConstructAndRegisterSingleton<
            TInterface,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TType>(
                this IMvxIoCProvider ioc, params object[] arguments)
                    where TInterface : class
                    where TType : class, TInterface
        {
            var instance = ioc.IoCConstruct<TType>(arguments);
            ioc.RegisterSingleton<TInterface>(instance);
            return instance;
        }

        public static object ConstructAndRegisterSingleton(
            this IMvxIoCProvider ioc,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type)
        {
            var instance = ioc.IoCConstruct(type);
            ioc.RegisterSingleton(type, instance);
            return instance;
        }

        public static object ConstructAndRegisterSingleton(
            this IMvxIoCProvider ioc,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type,
            IDictionary<string, object> arguments)
        {
            var instance = ioc.IoCConstruct(type, arguments);
            ioc.RegisterSingleton(type, instance);
            return instance;
        }

        public static object ConstructAndRegisterSingleton(
            this IMvxIoCProvider ioc,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type,
            object arguments)
        {
            var instance = ioc.IoCConstruct(type, arguments);
            ioc.RegisterSingleton(type, instance);
            return instance;
        }

        public static object ConstructAndRegisterSingleton(
            this IMvxIoCProvider ioc,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type,
            params object[] arguments)
        {
            var instance = ioc.IoCConstruct(type, arguments);
            ioc.RegisterSingleton(type, instance);
            return instance;
        }

        public static void LazyConstructAndRegisterSingleton<
            TInterface,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TType>(
                this IMvxIoCProvider ioc)
                    where TInterface : class
                    where TType : class, TInterface
        {
            ioc.RegisterSingleton<TInterface>(() => ioc.IoCConstruct<TType>());
        }

        public static void LazyConstructAndRegisterSingleton<TInterface>(
            this IMvxIoCProvider ioc,
            Func<TInterface> constructor)
                where TInterface : class
        {
            ioc.RegisterSingleton(constructor);
        }

        public static void LazyConstructAndRegisterSingleton(
            this IMvxIoCProvider ioc,
            Type type,
            Func<object> constructor)
        {
            ioc.RegisterSingleton(type, constructor);
        }

        public static void LazyConstructAndRegisterSingleton<
            TInterface,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter1>(
                this IMvxIoCProvider ioc,
                Func<TParameter1, TInterface> constructor)
                    where TInterface : class
                    where TParameter1 : class
        {
            var resolver = ioc.CreateResolver(constructor);
            ioc.RegisterSingleton(resolver);
        }

        public static void LazyConstructAndRegisterSingleton<
            TInterface,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter1,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter2>(
                this IMvxIoCProvider ioc,
                Func<TParameter1, TParameter2, TInterface> constructor)
                    where TInterface : class
                    where TParameter1 : class
                    where TParameter2 : class
        {
            var resolver = ioc.CreateResolver(constructor);
            ioc.RegisterSingleton(resolver);
        }

        public static void LazyConstructAndRegisterSingleton<
            TInterface,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter1,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter2,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter3>(
                this IMvxIoCProvider ioc,
                Func<TParameter1, TParameter2, TParameter3, TInterface> constructor)
                    where TInterface : class
                    where TParameter1 : class
                    where TParameter2 : class
                    where TParameter3 : class
        {
            var resolver = ioc.CreateResolver(constructor);
            ioc.RegisterSingleton(resolver);
        }

        public static void LazyConstructAndRegisterSingleton<
            TInterface,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter1,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter2,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter3,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter4>(
                this IMvxIoCProvider ioc, Func<TParameter1, TParameter2, TParameter3, TParameter4, TInterface> constructor)
                    where TInterface : class
                    where TParameter1 : class
                    where TParameter2 : class
                    where TParameter3 : class
                    where TParameter4 : class
        {
            var resolver = ioc.CreateResolver(constructor);
            ioc.RegisterSingleton(resolver);
        }

        public static void LazyConstructAndRegisterSingleton<
            TInterface,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter1,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter2,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter3,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter4,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter5>(
                this IMvxIoCProvider ioc,
                Func<TParameter1, TParameter2, TParameter3, TParameter4, TParameter5, TInterface> constructor)
                    where TInterface : class
                    where TParameter1 : class
                    where TParameter2 : class
                    where TParameter3 : class
                    where TParameter4 : class
                    where TParameter5 : class
        {
            var resolver = ioc.CreateResolver(constructor);
            ioc.RegisterSingleton(resolver);
        }

        public static void RegisterType<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TType>(
            this IMvxIoCProvider ioc)
                where TType : class
        {
            ioc.RegisterType<TType, TType>();
        }

        public static void RegisterType(
            this IMvxIoCProvider ioc,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type tType)
        {
            ioc.RegisterType(tType, tType);
        }

        public static void RegisterType<
            TInterface,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter1>(
                this IMvxIoCProvider ioc, Func<TParameter1, TInterface> constructor)
                    where TInterface : class
                    where TParameter1 : class
        {
            var resolver = ioc.CreateResolver(constructor);
            ioc.RegisterType(resolver);
        }

        public static void RegisterType<
            TInterface,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter1,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter2>(
                this IMvxIoCProvider ioc,
                Func<TParameter1, TParameter2, TInterface> constructor)
                    where TInterface : class
                    where TParameter1 : class
                    where TParameter2 : class
        {
            var resolver = ioc.CreateResolver(constructor);
            ioc.RegisterType(resolver);
        }

        public static void RegisterType<
            TInterface,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter1,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter2,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter3>(
                this IMvxIoCProvider ioc, Func<TParameter1, TParameter2, TParameter3, TInterface> constructor)
                    where TInterface : class
                    where TParameter1 : class
                    where TParameter2 : class
                    where TParameter3 : class
        {
            var resolver = ioc.CreateResolver(constructor);
            ioc.RegisterType(resolver);
        }

        public static void RegisterType<
            TInterface,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter1,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter2,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter3,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter4>(
                this IMvxIoCProvider ioc,
                Func<TParameter1, TParameter2, TParameter3, TParameter4, TInterface> constructor)
                    where TInterface : class
                    where TParameter1 : class
                    where TParameter2 : class
                    where TParameter3 : class
                    where TParameter4 : class
        {
            var resolver = ioc.CreateResolver(constructor);
            ioc.RegisterType(resolver);
        }

        public static void RegisterType<
            TInterface,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter1,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter2,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter3,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter4,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TParameter5>(
                this IMvxIoCProvider ioc,
                Func<TParameter1, TParameter2, TParameter3, TParameter4, TParameter5, TInterface> constructor)
                    where TInterface : class
                    where TParameter1 : class
                    where TParameter2 : class
                    where TParameter3 : class
                    where TParameter4 : class
                    where TParameter5 : class
        {
            var resolver = ioc.CreateResolver(constructor);
            ioc.RegisterType(resolver);
        }
    }
}
