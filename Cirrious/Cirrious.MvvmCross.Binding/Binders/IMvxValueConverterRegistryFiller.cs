// IMvxValueConverterRegistryFiller.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using Cirrious.CrossCore.Converters;

namespace Cirrious.MvvmCross.Binding.Binders
{
    public interface IMvxNamedInstanceRegistryFiller<T>
    {
        void FillFrom(IMvxNamedInstanceRegistry<T> registry, Type type);
        void FillFrom(IMvxNamedInstanceRegistry<T> registry, Assembly assembly);
    }
    public interface IMvxValueConverterRegistryFiller : IMvxNamedInstanceRegistryFiller<IMvxValueConverter>
    {        
    }
}