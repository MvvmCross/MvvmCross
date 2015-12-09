// IMvxValueConverterRegistryFiller.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.Platform;
using System;
using System.Reflection;

namespace Cirrious.MvvmCross.Binding.Binders
{
    public interface IMvxNamedInstanceRegistryFiller<out T>
    {
        void FillFrom(IMvxNamedInstanceRegistry<T> registry, Type type);

        void FillFrom(IMvxNamedInstanceRegistry<T> registry, Assembly assembly);
    }

    public interface IMvxValueConverterRegistryFiller : IMvxNamedInstanceRegistryFiller<IMvxValueConverter>
    {
    }
}