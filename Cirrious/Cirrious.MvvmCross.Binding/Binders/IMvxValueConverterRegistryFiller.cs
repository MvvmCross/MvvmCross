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