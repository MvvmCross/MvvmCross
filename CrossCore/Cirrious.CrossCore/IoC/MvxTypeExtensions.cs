// MvxTypeExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cirrious.CrossCore.IoC
{
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
                Mvx.Warning("ReflectionTypeLoadException masked during loading of {0} - error {1}",
                    assembly.FullName, e.ToLongString());
                return new Type[0];
            }
        }

        public static IEnumerable<Type> CreatableTypes(this Assembly assembly)
        {
            return assembly
                .ExceptionSafeGetTypes()
                .Select(t => t.GetTypeInfo())
                .Where(t => !t.IsAbstract)
                .Where(t => t.DeclaredConstructors.Any(c => !c.IsStatic && c.IsPublic))
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
            return types.Where(x => x.Namespace != null && x.Namespace.StartsWith(namespaceBase));
        }

        public static IEnumerable<Type> WithAttribute(this IEnumerable<Type> types, Type attributeType)
        {
            return types.Where(x => x.GetCustomAttributes(attributeType, true).Any());
        }

        public static IEnumerable<Type> WithAttribute<TAttribute>(this IEnumerable<Type> types)
            where TAttribute : Attribute
        {
            return types.WithAttribute(typeof(TAttribute));
        }

        public static IEnumerable<Type> Inherits(this IEnumerable<Type> types, Type baseType)
        {
            return types.Where(x => baseType.IsAssignableFrom(x));
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
                var lookup = except.ToDictionary(x => x, x => true);
                return types.Where(x => !lookup.ContainsKey(x));
            }
            else
            {
                return types.Where(x => !except.Contains(x));
            }
        }

        public class ServiceTypeAndImplementationTypePair
        {
            public ServiceTypeAndImplementationTypePair(List<Type> serviceTypes, Type implementationType)
            {
                ImplementationType = implementationType;
                ServiceTypes = serviceTypes;
            }

            public List<Type> ServiceTypes { get; private set; }
            public Type ImplementationType { get; private set; }
        }

        public static IEnumerable<ServiceTypeAndImplementationTypePair> AsTypes(this IEnumerable<Type> types)
        {
            return types.Select(t => new ServiceTypeAndImplementationTypePair(new List<Type>() { t }, t));
        }

        public static IEnumerable<ServiceTypeAndImplementationTypePair> AsInterfaces(this IEnumerable<Type> types) => types.Select(t => new ServiceTypeAndImplementationTypePair(t.GetInterfaces().ToList(), t));

        public static IEnumerable<ServiceTypeAndImplementationTypePair> AsInterfaces(this IEnumerable<Type> types, params Type[] interfaces)
        {
            // optimisation - if we have 3 or more interfaces, then use a dictionary
            if (interfaces.Length >= 3)
            {
                var lookup = interfaces.ToDictionary(x => x, x => true);
                return
                    types.Select(
                        t =>
                        new ServiceTypeAndImplementationTypePair(
                            t.GetInterfaces().Where(iface => lookup.ContainsKey(iface)).ToList(), t));
            }
            else
            {
                return
                    types.Select(
                        t =>
                        new ServiceTypeAndImplementationTypePair(
                            t.GetInterfaces().Where(iface => interfaces.Contains(iface)).ToList(), t));
            }
        }

        public static IEnumerable<ServiceTypeAndImplementationTypePair> ExcludeInterfaces(this IEnumerable<ServiceTypeAndImplementationTypePair> pairs, params Type[] toExclude)
        {
            foreach (var pair in pairs)
            {
                var excludedList = pair.ServiceTypes.Where(c => !toExclude.Contains(c)).ToList();
                if (excludedList.Any())
                {
                    var newPair = new ServiceTypeAndImplementationTypePair(
                        excludedList, pair.ImplementationType);
                    yield return newPair;
                }
            }
        }

        public static void RegisterAsSingleton(this IEnumerable<ServiceTypeAndImplementationTypePair> pairs)
        {
            foreach (var pair in pairs)
            {
                if (!pair.ServiceTypes.Any())
                    continue;

                var instance = Mvx.IocConstruct(pair.ImplementationType);
                foreach (var serviceType in pair.ServiceTypes)
                {
                    Mvx.RegisterSingleton(serviceType, instance);
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
                    Mvx.RegisterSingleton(serviceType, creationFunc);
                }
            }
        }

        public static void RegisterAsDynamic(this IEnumerable<ServiceTypeAndImplementationTypePair> pairs)
        {
            foreach (var pair in pairs)
            {
                foreach (var serviceType in pair.ServiceTypes)
                {
                    Mvx.RegisterType(serviceType, pair.ImplementationType);
                }
            }
        }

        public static object CreateDefault(this Type type)
        {
            if (type == null)
            {
                return null;
            }

            if (!type.GetTypeInfo().IsValueType)
            {
                return null;
            }

            if (Nullable.GetUnderlyingType(type) != null)
                return null;

            return Activator.CreateInstance(type);
        }
    }
}