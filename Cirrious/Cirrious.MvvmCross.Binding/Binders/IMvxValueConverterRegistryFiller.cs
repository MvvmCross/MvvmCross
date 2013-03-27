// IMvxValueConverterRegistryFiller.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;

namespace Cirrious.MvvmCross.Binding.Binders
{
    public interface IMvxValueConverterRegistryFiller
    {
        void FillFrom(IMvxValueConverterRegistry registry, Type type);
        void FillFrom(IMvxValueConverterRegistry registry, Assembly assembly);
    }
}