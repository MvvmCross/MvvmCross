// IMvxValueCombinerLookup.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Binding.Binders;

namespace MvvmCross.Binding.Combiners
{
    public interface IMvxValueCombinerLookup
        : IMvxNamedInstanceLookup<IMvxValueCombiner>
    {
    }
}