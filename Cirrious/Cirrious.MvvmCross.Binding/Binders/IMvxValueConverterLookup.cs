// IMvxValueConverterLookup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Converters;

namespace Cirrious.MvvmCross.Binding.Binders
{
    public interface IMvxNamedInstanceLookup<out T>
    {
        T Find(string name);
    }

    public interface IMvxValueConverterLookup
        : IMvxNamedInstanceLookup<IMvxValueConverter>
    {
    }
}