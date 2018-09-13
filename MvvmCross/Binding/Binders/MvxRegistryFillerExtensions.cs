// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Base;

namespace MvvmCross.Binding.Binders
{
    public static class MvxRegistryFillerExtensions
    {
        public static void Fill<T>(this IMvxNamedInstanceRegistry<T> registry, IEnumerable<Assembly> assemblies,
                                IEnumerable<Type> types)
            where T : class
        {
            var filler = Mvx.IoCProvider.Resolve<IMvxNamedInstanceRegistryFiller<T>>();
            registry.Fill(filler, assemblies);
            registry.Fill(filler, types);
        }

        public static void Fill<T>(this IMvxNamedInstanceRegistry<T> registry, IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null)
                return;

            var filler = Mvx.IoCProvider.Resolve<IMvxNamedInstanceRegistryFiller<T>>();
            registry.Fill(filler, assemblies);
        }

        public static void Fill<T>(this IMvxNamedInstanceRegistry<T> registry, IMvxNamedInstanceRegistryFiller<T> filler,
                                IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null)
                return;

            foreach (var assembly in assemblies)
            {
                registry.Fill(filler, assembly);
            }
        }

        public static void Fill<T>(this IMvxNamedInstanceRegistry<T> registry, Assembly assembly)
        {
            var filler = Mvx.IoCProvider.Resolve<IMvxNamedInstanceRegistryFiller<T>>();
            registry.Fill(filler, assembly);
        }

        public static void Fill<T>(this IMvxNamedInstanceRegistry<T> registry, IMvxNamedInstanceRegistryFiller<T> filler,
                                Assembly assembly)
        {
            filler.FillFrom(registry, assembly);
        }

        public static void Fill<T>(this IMvxNamedInstanceRegistry<T> registry, IEnumerable<Type> types)
        {
            if (types == null)
                return;

            var filler = Mvx.IoCProvider.Resolve<IMvxNamedInstanceRegistryFiller<T>>();
            registry.Fill(filler, types);
        }

        public static void Fill<T>(this IMvxNamedInstanceRegistry<T> registry, IMvxNamedInstanceRegistryFiller<T> filler,
                                IEnumerable<Type> types)
        {
            if (types == null)
                return;

            foreach (var type in types)
            {
                registry.Fill(filler, type);
            }
        }

        public static void Fill<T>(this IMvxNamedInstanceRegistry<T> registry, IMvxNamedInstanceRegistryFiller<T> filler,
                                Type type)
        {
            filler.FillFrom(registry, type);
        }

        public static void Fill<T>(this IMvxNamedInstanceRegistry<T> registry, Type type)
        {
            var filler = Mvx.IoCProvider.Resolve<IMvxNamedInstanceRegistryFiller<T>>();
            registry.Fill(filler, type);
        }
    }
}