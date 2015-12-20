// IMvxValueConverterLookup.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Binders
{
    using MvvmCross.Platform.Converters;

    public interface IMvxNamedInstanceLookup<out T>
    {
        T Find(string name);
    }

    public interface IMvxValueConverterLookup
        : IMvxNamedInstanceLookup<IMvxValueConverter>
    {
    }
}