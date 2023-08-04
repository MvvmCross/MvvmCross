// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using System.Diagnostics.CodeAnalysis;

namespace MvvmCross.IoC
{
    public interface IMvxIoCProvider
    {
        bool CanResolve<T>()
            where T : class;

        bool CanResolve(Type type);

        [RequiresUnreferencedCode("Calls public constructors on each target property type")]
        T? Resolve<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]T>()
            where T : class;

        [RequiresUnreferencedCode("Calls public constructors on each target property type")]
        object? Resolve([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]Type type);

        [RequiresUnreferencedCode("Calls public constructors on each target property type")]
        bool TryResolve<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]T>(out T? resolved)
            where T : class;

        [RequiresUnreferencedCode("Calls public constructors on each target property type")]
        bool TryResolve([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]Type type, out object? resolved);

        [RequiresUnreferencedCode("Calls public constructors on each target property type")]
        T? Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]T>()
            where T : class;

        [RequiresUnreferencedCode("Calls public constructors on each target property type")]
        object? Create([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]Type type);

        [RequiresUnreferencedCode("Calls public constructors on each target property type")]
        T? GetSingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]T>()
            where T : class;

        [RequiresUnreferencedCode("Calls public constructors on each target property type")]
        object? GetSingleton([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]Type type);

        void RegisterType<TFrom, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]TTo>()
            where TFrom : class
            where TTo : class, TFrom;

        void RegisterType<TInterface>(Func<TInterface> constructor)
            where TInterface : class;

        void RegisterType(Type t, Func<object> constructor);

        void RegisterType(Type tFrom, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]Type tTo);

        void RegisterSingleton<TInterface>(TInterface theObject)
            where TInterface : class;

        void RegisterSingleton(Type tInterface, object theObject);

        void RegisterSingleton<TInterface>(Func<TInterface> theConstructor)
            where TInterface : class;

        void RegisterSingleton(Type tInterface, Func<object> theConstructor);

        [RequiresUnreferencedCode("Calls public constructors on each target property type")]
        T? IoCConstruct<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]T>()
            where T : class;

        [RequiresUnreferencedCode("Calls public constructors on each target property type")]
        T? IoCConstruct<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]T>(IDictionary<string, object>? arguments)
            where T : class;

        [RequiresUnreferencedCode("Calls public constructors on each target property type")]
        T? IoCConstruct<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]T>(object? arguments)
            where T : class;

        [RequiresUnreferencedCode("Calls public constructors on each target property type")]
        T? IoCConstruct<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]T>(params object?[] arguments)
            where T : class;

        [RequiresUnreferencedCode("Calls public constructors on each target property type")]
        object? IoCConstruct([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]Type type);

        [RequiresUnreferencedCode("Calls public constructors on each target property type")]
        object? IoCConstruct([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]Type type, IDictionary<string, object>? arguments);

        [RequiresUnreferencedCode("Calls public constructors on each target property type")]
        object? IoCConstruct([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]Type type, object? arguments);

        [RequiresUnreferencedCode("Calls public constructors on each target property type")]
        object? IoCConstruct([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]Type type, params object?[] arguments);

        void CallbackWhenRegistered<T>(Action action);

        void CallbackWhenRegistered(Type type, Action action);

        [RequiresUnreferencedCode("Calls public constructors on each target property type")]
        IMvxIoCProvider CreateChildContainer();
    }
}
