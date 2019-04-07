// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace MvvmCross.IoC
{
    public interface IMvxIoCProvider
    {
        bool CanResolve<T>()
            where T : class;

        bool CanResolve(Type type);

        T Resolve<T>()
            where T : class;

        object Resolve(Type type);

        bool TryResolve<T>(out T resolved)
            where T : class;

        bool TryResolve(Type type, out object resolved);

        T Create<T>()
            where T : class;

        object Create(Type type);

        T GetSingleton<T>()
            where T : class;

        object GetSingleton(Type type);

        void RegisterType<TFrom, TTo>()
            where TFrom : class
            where TTo : class, TFrom;

        void RegisterType<TInterface>(Func<TInterface> constructor)
            where TInterface : class;

        void RegisterType(Type t, Func<object> constructor);

        void RegisterType(Type tFrom, Type tTo);

        void RegisterSingleton<TInterface>(TInterface theObject)
            where TInterface : class;

        void RegisterSingleton(Type tInterface, object theObject);

        void RegisterSingleton<TInterface>(Func<TInterface> theConstructor)
            where TInterface : class;

        void RegisterSingleton(Type tInterface, Func<object> theConstructor);

        T IoCConstruct<T>()
            where T : class;

        T IoCConstruct<T>(IDictionary<string, object> arguments)
            where T : class;

        T IoCConstruct<T>(object arguments)
            where T : class;

        T IoCConstruct<T>(params object[] arguments)
            where T : class;

        object IoCConstruct(Type type);

        object IoCConstruct(Type type, IDictionary<string, object> arguments);

        object IoCConstruct(Type type, object arguments);

        object IoCConstruct(Type type, params object[] arguments);

        void CallbackWhenRegistered<T>(Action action);

        void CallbackWhenRegistered(Type type, Action action);

        IMvxIoCProvider CreateChildContainer();
    }
}
