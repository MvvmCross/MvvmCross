// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MvvmCross.Base;

namespace MvvmCross.Binding.Binders
{
    public static class MvxRegistryFillerExtensions
    {
        [RequiresUnreferencedCode("This method uses reflection to check for creatable types, which may not be preserved by trimming")]
        public static void Fill<T>(
            this IMvxNamedInstanceRegistry<T> registry, IEnumerable<Assembly> assemblies, IEnumerable<Type> types)
            where T : notnull
        {
            var filler = Mvx.IoCProvider.Resolve<IMvxNamedInstanceRegistryFiller<T>>();
            registry.Fill(filler, assemblies);
            registry.Fill(filler, types);
        }

        [RequiresUnreferencedCode("This method uses reflection to check for creatable types, which may not be preserved by trimming")]
        public static void Fill<T>(this IMvxNamedInstanceRegistry<T> registry, IEnumerable<Assembly> assemblies)
            where T : notnull
        {
            if (assemblies == null)
                return;

            var filler = Mvx.IoCProvider.Resolve<IMvxNamedInstanceRegistryFiller<T>>();
            registry.Fill(filler, assemblies);
        }

        [RequiresUnreferencedCode("This method uses reflection to check for creatable types, which may not be preserved by trimming")]
        public static void Fill<T>(
            this IMvxNamedInstanceRegistry<T> registry, IMvxNamedInstanceRegistryFiller<T> filler,
            IEnumerable<Assembly> assemblies)
            where T : notnull
        {
            if (assemblies == null)
                return;

            foreach (var assembly in assemblies)
            {
                registry.Fill(filler, assembly);
            }
        }

        [RequiresUnreferencedCode("This method uses reflection to check for creatable types, which may not be preserved by trimming")]
        public static void Fill<T>(this IMvxNamedInstanceRegistry<T> registry, Assembly assembly)
            where T : notnull
        {
            var filler = Mvx.IoCProvider.Resolve<IMvxNamedInstanceRegistryFiller<T>>();
            registry.Fill(filler, assembly);
        }

        [RequiresUnreferencedCode("This method uses reflection to check for creatable types, which may not be preserved by trimming")]
        public static void Fill<T>(this IMvxNamedInstanceRegistry<T> registry, IMvxNamedInstanceRegistryFiller<T> filler,
                                Assembly assembly)
            where T : notnull
        {
            filler.FillFrom(registry, assembly);
        }

        [RequiresUnreferencedCode("This method uses reflection to check for creatable types, which may not be preserved by trimming")]
        public static void Fill<T>(this IMvxNamedInstanceRegistry<T> registry, IEnumerable<Type> types)
            where T : notnull
        {
            if (types == null)
                return;

            var filler = Mvx.IoCProvider.Resolve<IMvxNamedInstanceRegistryFiller<T>>();
            registry.Fill(filler, types);
        }

        [RequiresUnreferencedCode("This method uses reflection to check for creatable types, which may not be preserved by trimming")]
        public static void Fill<T>(
            this IMvxNamedInstanceRegistry<T> registry,
            IMvxNamedInstanceRegistryFiller<T> filler,
            IEnumerable<Type> types)
        {
            if (types == null)
                return;

            foreach (var type in types)
            {
                registry.Fill(filler, type);
            }
        }

        public static void Fill<T>(
            this IMvxNamedInstanceRegistry<T> registry, IMvxNamedInstanceRegistryFiller<T> filler,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicFields)] Type type)
            where T : notnull
        {
            filler.FillFrom(registry, type);
        }

        public static void Fill<T>(
            this IMvxNamedInstanceRegistry<T> registry,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicFields)] Type type)
            where T : notnull
        {
            var filler = Mvx.IoCProvider.Resolve<IMvxNamedInstanceRegistryFiller<T>>();
            registry.Fill(filler, type);
        }
    }
}
