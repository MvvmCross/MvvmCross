// IMvxValueConverterRegistry.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Converters
{
    using MvvmCross.Platform.Platform;

    public interface IMvxValueConverterRegistry : IMvxNamedInstanceRegistry<IMvxValueConverter>
    {
    }
}