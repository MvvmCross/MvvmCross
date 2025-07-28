// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Diagnostics.CodeAnalysis;

namespace MvvmCross.IoC;

public interface IMvxIoCProvider
{
    bool CanResolve<T>()
        where T : class;

    bool CanResolve(Type type);

    T? Resolve<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        where T : class;

    object? Resolve([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type);

    bool TryResolve<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(out T? resolved)
        where T : class;

    bool TryResolve([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type, out object? resolved);

    T? Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        where T : class;

    object? Create([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type);

    T? GetSingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        where T : class;

    object? GetSingleton([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type);

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

    T IoCConstruct<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        where T : class;

    T IoCConstruct<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(IDictionary<string, object>? arguments)
        where T : class;

    T IoCConstruct<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(object? arguments)
        where T : class;

    T IoCConstruct<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(params object?[] arguments)
        where T : class;

    object IoCConstruct([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type);

    object IoCConstruct([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type, IDictionary<string, object>? arguments);

    object IoCConstruct([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type, object? arguments);

    object IoCConstruct([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type, params object?[] arguments);

    IMvxIoCProvider CreateChildContainer();
}
