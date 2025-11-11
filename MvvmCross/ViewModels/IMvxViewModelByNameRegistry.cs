// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace MvvmCross.ViewModels
{
#nullable enable
    public interface IMvxViewModelByNameRegistry
    {
        void Add(Type viewModelType);

        void Add<TViewModel>() where TViewModel : IMvxViewModel;

        [RequiresUnreferencedCode("This method registers view models that may not be preserved by trimming")]
        void AddAll(Assembly assembly);
    }
#nullable restore
}
