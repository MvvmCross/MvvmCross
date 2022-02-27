// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Tvos.Views
{
    public interface IMvxTvosViewCreator : IMvxCurrentRequest
    {
        IMvxTvosView CreateView(MvxViewModelRequest request);

        IMvxTvosView CreateView(IMvxViewModel viewModel);

        IMvxTvosView CreateViewOfType(Type viewType, MvxViewModelRequest request);
    }
}
