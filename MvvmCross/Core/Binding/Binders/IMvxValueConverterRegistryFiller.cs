// IMvxValueConverterRegistryFiller.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Binders
{
    using System;
    using System.Reflection;

    using MvvmCross.Platform.Converters;
    using MvvmCross.Platform.Platform;

    public interface IMvxNamedInstanceRegistryFiller<out T>
    {
        string FindName(Type type);

        void FillFrom(IMvxNamedInstanceRegistry<T> registry, Type type);

        void FillFrom(IMvxNamedInstanceRegistry<T> registry, Assembly assembly);
    }

    public interface IMvxValueConverterRegistryFiller : IMvxNamedInstanceRegistryFiller<IMvxValueConverter>
    {
    }
}