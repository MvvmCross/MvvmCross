// MvxValueCombinerRegistryFiller.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Combiners
{
    using System;

    using MvvmCross.Binding.Binders;

    public class MvxValueCombinerRegistryFiller
        : MvxNamedInstanceRegistryFiller<IMvxValueCombiner>
          , IMvxValueCombinerRegistryFiller
    {
        protected override string FindName(Type type)
        {
            var name = base.FindName(type);
            name = RemoveTail(name, "ValueCombiner");
            name = RemoveTail(name, "Combiner");
            return name;
        }
    }
}