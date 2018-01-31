// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;

namespace MvvmCross.Core.ViewModels
{
    public interface IMvxViewModelByNameRegistry
    {
        void Add(Type viewModelType);

        void Add<TViewModel>() where TViewModel : IMvxViewModel;

        void AddAll(Assembly assembly);
    }
}