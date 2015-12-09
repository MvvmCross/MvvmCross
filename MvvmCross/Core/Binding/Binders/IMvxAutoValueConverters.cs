// IMvxAutoValueConverters.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Converters;
using System;

namespace Cirrious.MvvmCross.Binding.Binders
{
    public interface IMvxAutoValueConverters
    {
        IMvxValueConverter Find(Type viewModelType, Type viewType);

        void Register(Type viewModelType, Type viewType, IMvxValueConverter converter);
    }
}