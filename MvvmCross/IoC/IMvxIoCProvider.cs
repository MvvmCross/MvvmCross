// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MvvmCross.IoC
{
    public interface IMvxIoCProvider
    {
        bool CanResolve<T>()
            where T : class;

        bool CanResolve(Type type);

        [RequiresUnreferencedCode("Resolve is not compatible with trimming")]
        T Resolve<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
            where T : class;

        [RequiresUnreferencedCode("Resolve is not compatible with trimming")]
        object Resolve([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type);

        [RequiresUnreferencedCode("TryResolve is not compatible with trimming")]
        bool TryResolve<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(out T resolved)
            where T : class;

        [RequiresUnreferencedCode("TryResolve is not compatible with trimming")]
        bool TryResolve([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type, out object resolved);

        [RequiresUnreferencedCode("Create is not compatible with trimming")]
        T Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
            where T : class;

        [RequiresUnreferencedCode("Create is not compatible with trimming")]
        object Create([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type);

        [RequiresUnreferencedCode("GetSingleton is not compatible with trimming")]
        T GetSingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
            where T : class;

        [RequiresUnreferencedCode("GetSingleton is not compatible with trimming")]
        object GetSingleton([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type);

        void RegisterType<TFrom, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTo>()
            where TFrom : class
            where TTo : class, TFrom;

        void RegisterType<TInterface>(Func<TInterface> constructor)
            where TInterface : class;

        void RegisterType(Type t, Func<object> constructor);

        void RegisterType(Type tFrom, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type tTo);

        void RegisterSingleton<TInterface>(TInterface theObject)
            where TInterface : class;

        void RegisterSingleton(Type tInterface, object theObject);

        void RegisterSingleton<TInterface>(Func<TInterface> theConstructor)
            where TInterface : class;

        void RegisterSingleton(Type tInterface, Func<object> theConstructor);

        [RequiresUnreferencedCode("IoCConstruct is incompatible with trimming")]
        T IoCConstruct<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
            where T : class;

        [RequiresUnreferencedCode("IoCConstruct is incompatible with trimming")]
        T IoCConstruct<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(IDictionary<string, object> arguments)
            where T : class;

        [RequiresUnreferencedCode("IoCConstruct is incompatible with trimming")]
        T IoCConstruct<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(object arguments)
            where T : class;

        [RequiresUnreferencedCode("IoCConstruct is incompatible with trimming")]
        T IoCConstruct<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(params object[] arguments)
            where T : class;

        [RequiresUnreferencedCode("IoCConstruct is incompatible with trimming")]
        object IoCConstruct([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type);

        [RequiresUnreferencedCode("IoCConstruct is incompatible with trimming")]
        object IoCConstruct([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type, IDictionary<string, object> arguments);

        [RequiresUnreferencedCode("IoCConstruct is incompatible with trimming")]
        object IoCConstruct([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type, object arguments);

        [RequiresUnreferencedCode("IoCConstruct is incompatible with trimming")]
        object IoCConstruct([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type, params object[] arguments);

        void CallbackWhenRegistered<T>(Action action);

        void CallbackWhenRegistered(Type type, Action action);

        IMvxIoCProvider CreateChildContainer();
    }
}
