// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MvvmCross.Base;

namespace MvvmCross.IoC
{
    /// <summary>
    /// Singleton IoC Provider.
    /// 
    /// Delegates to the <see cref="MvxIoCContainer"/> implementation
    /// </summary>
    public class MvxIoCProvider
        : MvxSingleton<IMvxIoCProvider>, IMvxIoCProvider
    {
        public static IMvxIoCProvider Initialize(IMvxIocOptions options = null)
        {
            if (Instance != null)
            {
                return Instance;
            }

            // create a new ioc container - it will register itself as the singleton
            // ReSharper disable ObjectCreationAsStatement
            new MvxIoCProvider(options);
            // ReSharper restore ObjectCreationAsStatement
            return Instance;
        }

        private readonly MvxIoCContainer _provider;

        protected MvxIoCProvider(IMvxIocOptions options)
        {
            _provider = new MvxIoCContainer(options);
        }

        public bool CanResolve<T>()
            where T : class
        {
            return _provider.CanResolve<T>();
        }

        public bool CanResolve(Type t)
        {
            return _provider.CanResolve(t);
        }

        public bool TryResolve<T>(out T resolved)
            where T : class
        {
            return _provider.TryResolve<T>(out resolved);
        }

        public bool TryResolve(Type type, out object resolved)
        {
            return _provider.TryResolve(type, out resolved);
        }

        public T Resolve<T>()
            where T : class
        {
            return _provider.Resolve<T>();
        }

        public object Resolve(Type t)
        {
            return _provider.Resolve(t);
        }

        public T GetSingleton<T>()
            where T : class
        {
            return _provider.GetSingleton<T>();
        }

        public object GetSingleton(Type t)
        {
            return _provider.GetSingleton(t);
        }

        public T Create<T>()
            where T : class
        {
            return _provider.Create<T>();
        }

        public object Create(Type t)
        {
            return _provider.Create(t);
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

        public void RegisterType(Type interfaceType, Type constructType)
        {
            _provider.RegisterType(interfaceType, constructType);
        }

        public void RegisterSingleton<TInterface>(TInterface theObject)
            where TInterface : class
        {
            _provider.RegisterSingleton(theObject);
        }

        public void RegisterSingleton(Type interfaceType, object theObject)
        {
            _provider.RegisterSingleton(interfaceType, theObject);
        }

        public void RegisterSingleton<TInterface>(Func<TInterface> theConstructor)
            where TInterface : class
        {
            _provider.RegisterSingleton(theConstructor);
        }

        public void RegisterSingleton(Type interfaceType, Func<object> theConstructor)
        {
            _provider.RegisterSingleton(interfaceType, theConstructor);
        }

        public T IoCConstruct<T>()
            where T : class
        {
            return _provider.IoCConstruct<T>();
        }

        public virtual object IoCConstruct(Type type)
        {
            return _provider.IoCConstruct(type);
        }

        public T IoCConstruct<T>(IDictionary<string, object> arguments) where T : class
        {
            return _provider.IoCConstruct<T>(arguments);
        }

        public T IoCConstruct<T>(params object[] arguments) where T : class
        {
            return _provider.IoCConstruct<T>(arguments);
        }

        public T IoCConstruct<T>(object arguments) where T : class
        {
            return _provider.IoCConstruct<T>(arguments);
        }

        public object IoCConstruct(Type type, IDictionary<string, object> arguments = null)
        {
            return _provider.IoCConstruct(type, arguments);
        }

        public object IoCConstruct(Type type, object arguments)
        {
            return _provider.IoCConstruct(type, arguments);
        }

        public object IoCConstruct(Type type, params object[] arguments)
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
