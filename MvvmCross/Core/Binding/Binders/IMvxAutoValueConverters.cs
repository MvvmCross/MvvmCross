// IMvxAutoValueConverters.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Platform.Converters;

namespace MvvmCross.Binding.Binders
{
    public interface IMvxAutoValueConverters
    {
        IMvxValueConverter Find(Type viewModelType, Type viewType);

        void Register(Type viewModelType, Type viewType, IMvxValueConverter converter);
    }
}