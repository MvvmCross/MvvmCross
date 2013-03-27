// MvxTypeExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cirrious.CrossCore.IoC
{
    public static class MvxTypeExtensions
    {
        public static IEnumerable<Type> CreatableTypes(this Assembly assembly)
        {
            return assembly
                .GetTypes()
                .Where(t => !t.IsAbstract)
                .Where(t => t.GetConstructors(BindingFlags.Instance | BindingFlags.Public).Any());
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
            return types.WithAttribute(typeof (TAttribute));
        }

        public static IEnumerable<Type> Inherits(this IEnumerable<Type> types, Type baseType)
        {
            return types.Where(x => baseType.IsAssignableFrom(x));
        }

        public static IEnumerable<Type> Inherits<TBase>(this IEnumerable<Type> types)
            where TBase : Attribute
        {
            return types.Inherits(typeof (TBase));
        }

        public static IEnumerable<Type> DoesNotInherit(this IEnumerable<Type> types, Type baseType)
        {
            return types.Where(x => !baseType.IsAssignableFrom(x));
        }

        public static IEnumerable<Type> DoesNotInherit<TBase>(this IEnumerable<Type> types)
            where TBase : Attribute
        {
            return types.DoesNotInherit(typeof (TBase));
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
            public ServiceTypeAndImplementationTypePair(Type serviceType, Type implementationType)
            {
                ImplementationType = implementationType;
                ServiceType = serviceType;
            }

            public Type ServiceType { get; private set; }
            public Type ImplementationType { get; private set; }
        }

        public static IEnumerable<ServiceTypeAndImplementationTypePair> AsTypes(this IEnumerable<Type> types)
        {
            return types.Select(t => new ServiceTypeAndImplementationTypePair(t, t));
        }

        public static IEnumerable<ServiceTypeAndImplementationTypePair> AsInterfaces(this IEnumerable<Type> types)
        {
            return types.SelectMany(t =>
                                    from iface in t.GetInterfaces()
                                    select new ServiceTypeAndImplementationTypePair(iface, t));
        }

        public static IEnumerable<ServiceTypeAndImplementationTypePair> AsInterfaces(this IEnumerable<Type> types, params Type[] interfaces)
        {
            // optimisation - if we have 3 or more interfaces, then use a dictionary
            if (interfaces.Length >= 3)
            {
                var lookup = interfaces.ToDictionary(x => x, x => true);
                return types.SelectMany(t =>
                                        from iface in t.GetInterfaces()
                                        where lookup.ContainsKey(iface)
                                        select new ServiceTypeAndImplementationTypePair(iface, t));
            }
            else
            {
                return types.SelectMany(t =>
                                        from iface in t.GetInterfaces()
                                        where interfaces.Contains(iface)
                                        select new ServiceTypeAndImplementationTypePair(iface, t));
            }
        }

        public static void RegisterAsSingleton(this IEnumerable<ServiceTypeAndImplementationTypePair> pairs)
        {
            foreach (var interfaceAndTypePair in pairs)
            {
                var instance = Mvx.IocConstruct(interfaceAndTypePair.ImplementationType);
                Mvx.RegisterSingleton(interfaceAndTypePair.ServiceType, instance);
            }
        }

        public static void RegisterAsLazySingleton(this IEnumerable<ServiceTypeAndImplementationTypePair> pairs)
        {
            foreach (var interfaceAndTypePair in pairs)
            {
                var typeToCreate = interfaceAndTypePair.ImplementationType;
                var creator = new Func<object>(() => Mvx.IocConstruct(typeToCreate));
                Mvx.RegisterSingleton(interfaceAndTypePair.ServiceType, creator);
            }
        }

        public static void RegisterAsDynamic(this IEnumerable<ServiceTypeAndImplementationTypePair> pairs)
        {
            foreach (var interfaceAndTypePair in pairs)
            {
                Mvx.RegisterType(interfaceAndTypePair.ServiceType, interfaceAndTypePair.ImplementationType);
            }
        }
    }
}