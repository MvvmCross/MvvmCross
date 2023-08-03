// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using System.Diagnostics.CodeAnalysis;
using MvvmCross.Base;

namespace MvvmCross.IoC
{
    /// <summary>
    /// Singleton IoC Provider.
    ///
    /// Delegates to the <see cref="MvxIoCContainer"/> implementation
    /// </summary>
    public sealed class MvxIoCProvider
        : MvxSingleton<IMvxIoCProvider>, IMvxIoCProvider
    {
        public static IMvxIoCProvider Initialize(IMvxIocOptions? options = null)
        {
            if (Instance != null)
            {
                return Instance;
            }

            // create a new ioc container - it will register itself as the singleton
            // ReSharper disable ObjectCreationAsStatement
            var instance = new MvxIoCProvider(options);

            // ReSharper restore ObjectCreationAsStatement
            return instance;
        }

        private readonly MvxIoCContainer _provider;

        private MvxIoCProvider(IMvxIocOptions? options)
        {
            _provider = new MvxIoCContainer(options);
        }

        public bool CanResolve<T>()
            where T : class
        {
            return _provider.CanResolve<T>();
        }

        public bool CanResolve(Type type)
        {
            return _provider.CanResolve(type);
        }

        public bool TryResolve<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]T>(out T? resolved)
            where T : class
        {
            return _provider.TryResolve(out resolved);
        }

        public bool TryResolve([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]Type type, out object? resolved)
        {
            return _provider.TryResolve(type, out resolved);
        }

        public T? Resolve<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]T>()
            where T : class
        {
            return _provider.Resolve<T>();
        }

        public object? Resolve([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]Type type)
        {
            return _provider.Resolve(type);
        }

        public T? GetSingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]T>()
            where T : class
        {
            return _provider.GetSingleton<T>();
        }

        public object? GetSingleton([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]Type type)
        {
            return _provider.GetSingleton(type);
        }

        public T? Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]T>()
            where T : class
        {
            return _provider.Create<T>();
        }

        public object? Create([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]Type type)
        {
            return _provider.Create(type);
        }

        public void RegisterType<TInterface, TToConstruct>()
            where TInterface : class
            where TToConstruct : class, TInterface
        {
            _provider.RegisterType<TInterface, TToConstruct>();
        }

        public void RegisterType<TInterface>(Func<TInterface> constructor)
            where TInterface : class
        {
            _provider.RegisterType(constructor);
        }

        public void RegisterType(Type t, Func<object> constructor)
        {
            _provider.RegisterType(t, constructor);
        }

        public void RegisterType(Type tFrom, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]Type tTo)
        {
            _provider.RegisterType(tFrom, tTo);
        }

        public void RegisterSingleton<TInterface>(TInterface theObject)
            where TInterface : class
        {
            _provider.RegisterSingleton(theObject);
        }

        public void RegisterSingleton(Type tInterface, object theObject)
        {
            _provider.RegisterSingleton(tInterface, theObject);
        }

        public void RegisterSingleton<TInterface>(Func<TInterface> theConstructor)
            where TInterface : class
        {
            _provider.RegisterSingleton(theConstructor);
        }

        public void RegisterSingleton(Type tInterface, Func<object> theConstructor)
        {
            _provider.RegisterSingleton(tInterface, theConstructor);
        }

        public T? IoCConstruct<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]T>() where T : class
        {
            return _provider.IoCConstruct<T>((IDictionary<string, object>?)null);
        }

        public T? IoCConstruct<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]T>(IDictionary<string, object>? arguments) where T : class
        {
            return _provider.IoCConstruct<T>(arguments);
        }

        public T? IoCConstruct<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]T>(params object?[] arguments) where T : class
        {
            return _provider.IoCConstruct<T>(arguments);
        }

        public T? IoCConstruct<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]T>(object? arguments) where T : class
        {
            return _provider.IoCConstruct<T>(arguments);
        }

        public object? IoCConstruct([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]Type type)
        {
            return _provider.IoCConstruct(type, (IDictionary<string, object>?)null);
        }

        public object? IoCConstruct([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]Type type, IDictionary<string, object>? arguments)
        {
            return _provider.IoCConstruct(type, arguments);
        }

        public object? IoCConstruct([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]Type type, object? arguments)
        {
            return _provider.IoCConstruct(type, arguments);
        }

        public object? IoCConstruct([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]Type type, params object?[] arguments)
        {
            return _provider.IoCConstruct(type, arguments);
        }

        public void CallbackWhenRegistered<T>(Action action)
        {
            _provider.CallbackWhenRegistered<T>(action);
        }

        public void CallbackWhenRegistered(Type type, Action action)
        {
            _provider.CallbackWhenRegistered(type, action);
        }

        public void CleanAllResolvers()
        {
            _provider.CleanAllResolvers();
        }

        public IMvxIoCProvider CreateChildContainer()
        {
            return _provider.CreateChildContainer();
        }
    }
}
