// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.ViewModels;

namespace MvvmCross.ViewModels
{
    public interface IMvxChildViewModelCache
    {
        int Cache(IMvxViewModel viewModel);

        IMvxViewModel Get(int index);

        IMvxViewModel Get(Type viewModelType);

        void Remove(int index);

        void Remove(Type viewModelType);

        bool Exists(Type viewModelType);
    }
}
