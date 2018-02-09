// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Base.IoC
{
    public interface IMvxIoCProvider
    {
        bool CanResolve<T>()
            where T : class;

        bool CanResolve(Type type);

        T Resolve<T>()
            where T : class;

        object Resolve(Type type);

        T Create<T>()
            where T : class;

        object Create(Type type);

        T GetSingleton<T>()
            where T : class;

        object GetSingleton(Type type);

        bool TryResolve<T>(out T resolved)
            where T : class;

        bool TryResolve(Type type, out object resolved);

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

        object IoCConstruct(Type type);

        void CallbackWhenRegistered<T>(Action action);

        void CallbackWhenRegistered(Type type, Action action);

        IMvxIoCProvider CreateChildContainer();
    }
}
