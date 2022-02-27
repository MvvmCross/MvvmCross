// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Converters;

namespace MvvmCross.Binding.Binders
{
    public interface IMvxAutoValueConverters
    {
        IMvxValueConverter Find(Type viewModelType, Type viewType);

        void Register(Type viewModelType, Type viewType, IMvxValueConverter converter);
    }
}
