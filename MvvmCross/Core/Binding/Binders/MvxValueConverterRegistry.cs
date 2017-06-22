// MvxValueConverterRegistry.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform.Converters;

namespace MvvmCross.Binding.Binders
{
    public class MvxValueConverterRegistry
        : MvxNamedInstanceRegistry<IMvxValueConverter>, IMvxValueConverterLookup, IMvxValueConverterRegistry
    {
    }
}