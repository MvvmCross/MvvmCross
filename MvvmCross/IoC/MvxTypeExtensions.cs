// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Logging;
using MvvmCross.Logging;

namespace MvvmCross.IoC;

public static class MvxTypeExtensions
{
    public static IEnumerable<Type> ExceptionSafeGetTypes(this Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException e)
        {
            MvxLogHost.Default?.LogWarning(e,
                "ReflectionTypeLoadException masked during loading of {AssemblyName}",
                assembly.FullName);

            foreach (var exception in e.LoaderExceptions)
            {
                MvxLogHost.Default?.LogWarning(exception, "Failed to load type");
            }

            if (Debugger.IsAttached)
                Debugger.Break();

            return Enumerable.Empty<Type>();
        }
    }

    public static IEnumerable<Type> CreatableTypes(this Assembly assembly)
    {
        return assembly
            .ExceptionSafeGetTypes()
            .Select(t => t.GetTypeInfo())
            .Where(t => !t.IsAbstract && t.DeclaredConstructors.Any(c => c is { IsStatic: false, IsPublic: true }))
            .Select(t => t.AsType());
    }

    public static IEnumerable<Type> EndingWith(this IEnumerable<Type> types, string endingWith)
    {
        return types.Where(x => x.Name.EndsWith(endingWith));
    }

    public static IEnumerable<Type> StartingWith(this IEnumerable<Type> types, string endingWith)
    {
        return types.Where(x => x.Name.StartsWith(endingWith));
    }

    public static IEnumerable<Type> Containing(this IEnumerable<Type> types, string containing)
    {
        return types.Where(x => x.Name.Contains(containing));
    }

    public static IEnumerable<Type> InNamespace(this IEnumerable<Type> types, string namespaceBase)
    {
        return types.Where(x => x.Namespace?.StartsWith(namespaceBase) == true);
    }

    public static IEnumerable<Type> WithAttribute(this IEnumerable<Type> types, Type attributeType)
    {
        return types.Where(x => x.GetCustomAttributes(attributeType, true).Length > 0);
    }

    public static IEnumerable<Type> WithAttribute<TAttribute>(this IEnumerable<Type> types)
        where TAttribute : Attribute
    {
        return types.WithAttribute(typeof(TAttribute));
    }

    public static IEnumerable<Type> Inherits(this IEnumerable<Type> types, Type baseType)
    {
        return types.Where(baseType.IsAssignableFrom);
    }

    public static IEnumerable<Type> Inherits<TBase>(this IEnumerable<Type> types)
    {
        return types.Inherits(typeof(TBase));
    }

    public static IEnumerable<Type> DoesNotInherit(this IEnumerable<Type> types, Type baseType)
    {
        return types.Where(x => !baseType.IsAssignableFrom(x));
    }

    public static IEnumerable<Type> DoesNotInherit<TBase>(this IEnumerable<Type> types)
        where TBase : Attribute
    {
        return types.DoesNotInherit(typeof(TBase));
    }

    public static IEnumerable<Type> Except(this IEnumerable<Type> types, params Type[] except)
    {
        // optimisation - if we have 3 or more except cases, then use a dictionary
        if (except.Length >= 3)
        {
            var lookup = except.ToDictionary(x => x, _ => true);
            return types.Where(x => !lookup.ContainsKey(x));
        }

        return types.Where(x => !except.Contains(x));
    }

    public static bool IsGenericPartiallyClosed(this Type type) =>
        type.GetTypeInfo().IsGenericType &&
        type.GetTypeInfo().ContainsGenericParameters &&
        type.GetGenericTypeDefinition() != type;

    public class ServiceTypeAndImplementationTypePair
    {
        public List<Type> ServiceTypes { get; }
        public Type ImplementationType { get; }

        public ServiceTypeAndImplementationTypePair(List<Type> serviceTypes, Type implementationType)
        {
            ImplementationType = implementationType;
            ServiceTypes = serviceTypes;
        }
    }

    public static IEnumerable<ServiceTypeAndImplementationTypePair> AsTypes(this IEnumerable<Type> types)
    {
        return types.Select(t => new ServiceTypeAndImplementationTypePair([t], t));
    }

    public static IEnumerable<ServiceTypeAndImplementationTypePair> AsInterfaces(this IEnumerable<Type> types) =>
        types.Select(t => new ServiceTypeAndImplementationTypePair(t.GetInterfaces().ToList(), t));

    public static IEnumerable<ServiceTypeAndImplementationTypePair> AsInterfaces(this IEnumerable<Type> types, params Type[] interfaces)
    {
        // optimisation - if we have 3 or more interfaces, then use a dictionary
        if (interfaces.Length >= 3)
        {
            var lookup = interfaces.ToDictionary(x => x, _ => true);
            return
                types.Select(
                    t =>
                        new ServiceTypeAndImplementationTypePair(
                            t.GetInterfaces().Where(iface => lookup.ContainsKey(iface)).ToList(), t));
        }

        return
            types.Select(
                t =>
                    new ServiceTypeAndImplementationTypePair(
                        t.GetInterfaces().Where(interfaces.Contains).ToList(), t));
    }

    public static IEnumerable<ServiceTypeAndImplementationTypePair> ExcludeInterfaces(
        this IEnumerable<ServiceTypeAndImplementationTypePair> pairs, params Type[] toExclude)
    {
        return pairs
            .Select(pair =>
                new { pair, excludedList = pair.ServiceTypes.Where(c => !toExclude.Contains(c)).ToList() })
            .Where(t => t.excludedList.Count > 0)
            .Select(t => new ServiceTypeAndImplementationTypePair(t.excludedList, t.pair.ImplementationType));
    }

    public static void RegisterAsSingleton(this IEnumerable<ServiceTypeAndImplementationTypePair> pairs)
    {
        foreach (var pair in pairs)
        {
            if (pair.ServiceTypes.Count == 0)
                continue;

            var instance = Mvx.IoCProvider?.IoCConstruct(pair.ImplementationType, (object?)null);
            if (instance == null)
                continue;

            foreach (var serviceType in pair.ServiceTypes)
            {
                Mvx.IoCProvider?.RegisterSingleton(serviceType, instance);
            }
        }
    }

    public static void RegisterAsLazySingleton(this IEnumerable<ServiceTypeAndImplementationTypePair> pairs)
    {
        foreach (var pair in pairs)
        {
            if (!pair.ServiceTypes.Any())
                continue;

            var typeToCreate = pair.ImplementationType;
            var creator = new MvxLazySingletonCreator(typeToCreate);
            var creationFunc = new Func<object>(() => creator.Instance);
            foreach (var serviceType in pair.ServiceTypes)
            {
                Mvx.IoCProvider?.RegisterSingleton(serviceType, creationFunc);
            }
        }
    }

    public static void RegisterAsDynamic(this IEnumerable<ServiceTypeAndImplementationTypePair> pairs)
    {
        foreach (var pair in pairs)
        {
            foreach (var serviceType in pair.ServiceTypes)
            {
                Mvx.IoCProvider?.RegisterType(serviceType, pair.ImplementationType);
            }
        }
    }

    public static object? CreateDefault(this Type? type)
    {
        if (type == null)
            return null;

        if (!type.GetTypeInfo().IsValueType)
        {
            return null;
        }

        if (Nullable.GetUnderlyingType(type) != null)
            return null;

        return Activator.CreateInstance(type);
    }

    public static ConstructorInfo? FindApplicableConstructor(this Type type, IDictionary<string, object>? arguments)
    {
        var constructors = type.GetConstructors();
        if (arguments == null || arguments.Count == 0)
        {
            return constructors.MinBy(c => c.GetParameters().Length);
        }

        var unusedKeys = new List<string>(arguments.Keys);
        foreach (var constructor in constructors)
        {
            CheckConstructors(arguments, constructor, ref unusedKeys);

            if (unusedKeys.Count == 0)
            {
                return constructor;
            }
        }

        return null;
    }

    public static ConstructorInfo? FindApplicableConstructor(this Type type, object?[] arguments)
    {
        var constructors = type.GetConstructors();
        if (arguments.Length == 0)
        {
            return constructors.MinBy(c => c.GetParameters().Length);
        }

        foreach (var constructor in constructors)
        {
            var parameterTypes = constructor.GetParameters().Select(p => p.ParameterType);
            var unusedArguments = arguments.ToList();

            foreach (var parameterType in parameterTypes)
            {
                var argumentMatch = unusedArguments.Find(arg => parameterType.IsInstanceOfType(arg));
                if (argumentMatch != null)
                {
                    unusedArguments.Remove(argumentMatch);
                }
            }

            if (unusedArguments.Count == 0)
            {
                return constructor;
            }
        }

        return null;
    }
    
    private static void CheckConstructors(
        IDictionary<string, object> arguments, MethodBase constructor, ref List<string> unusedKeys)
    {
        foreach (var parameter in constructor.GetParameters())
        {
            if (parameter.Name != null &&
                unusedKeys.Contains(parameter.Name) &&
                parameter.ParameterType.IsInstanceOfType(arguments[parameter.Name]))
            {
                unusedKeys.Remove(parameter.Name);
            }
        }
    }
}
