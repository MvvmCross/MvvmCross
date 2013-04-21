// MvxRegistryFillerExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Reflection;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Converters;

namespace Cirrious.MvvmCross.Binding.Binders
{
    public static class MvxRegistryFillerExtensions
    {
        public static void Fill(this IMvxValueConverterRegistry registry, IEnumerable<Assembly> assemblies,
                                IEnumerable<Type> types)
        {
            var filler = Mvx.Resolve<IMvxValueConverterRegistryFiller>();
            registry.Fill(filler, assemblies);
            registry.Fill(filler, types);
        }

        public static void Fill(this IMvxValueConverterRegistry registry, IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null)
                return;

            var filler = Mvx.Resolve<IMvxValueConverterRegistryFiller>();
            registry.Fill(filler, assemblies);
        }

        public static void Fill(this IMvxValueConverterRegistry registry, IMvxValueConverterRegistryFiller filler,
                                IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null)
                return;

            foreach (var assembly in assemblies)
            {
                registry.Fill(filler, assembly);
            }
        }

        public static void Fill(this IMvxValueConverterRegistry registry, Assembly assembly)
        {
            var filler = Mvx.Resolve<IMvxValueConverterRegistryFiller>();
            registry.Fill(filler, assembly);
        }

        public static void Fill(this IMvxValueConverterRegistry registry, IMvxValueConverterRegistryFiller filler,
                                Assembly assembly)
        {
            filler.FillFrom(registry, assembly);
        }

        public static void Fill(this IMvxValueConverterRegistry registry, IEnumerable<Type> types)
        {
            if (types == null)
                return;

            var filler = Mvx.Resolve<IMvxValueConverterRegistryFiller>();
            registry.Fill(filler, types);
        }

        public static void Fill(this IMvxValueConverterRegistry registry, IMvxValueConverterRegistryFiller filler,
                                IEnumerable<Type> types)
        {
            if (types == null)
                return;

            foreach (var type in types)
            {
                registry.Fill(filler, type);
            }
        }

        public static void Fill(this IMvxValueConverterRegistry registry, IMvxValueConverterRegistryFiller filler,
                                Type type)
        {
            filler.FillFrom(registry, type);
        }

        public static void Fill(this IMvxValueConverterRegistry registry, Type type)
        {
            var filler = Mvx.Resolve<IMvxValueConverterRegistryFiller>();
            registry.Fill(filler, type);
        }
    }
}