// IMvxValueCombinerRegistry.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Binding.Combiners
{
    public interface IMvxValueCombinerRegistry
        : IMvxNamedInstanceRegistry<IMvxValueCombiner>
          , IMvxValueCombinerLookup
    {
    }
}