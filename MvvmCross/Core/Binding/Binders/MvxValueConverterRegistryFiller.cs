// MvxValueConverterRegistryFiller.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Binders
{
    using System;

    using MvvmCross.Platform.Converters;

    public class MvxValueConverterRegistryFiller
        : MvxNamedInstanceRegistryFiller<IMvxValueConverter>
          , IMvxValueConverterRegistryFiller
    {
        public override string FindName(Type type)
        {
            var name = base.FindName(type);
            name = RemoveTail(name, "ValueConverter");
            name = RemoveTail(name, "Converter");
            return name;
        }
    }
}