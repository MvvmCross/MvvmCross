// MvxValueCombinerRegistryFiller.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Binding.Binders;

namespace Cirrious.MvvmCross.Binding.Combiners
{
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