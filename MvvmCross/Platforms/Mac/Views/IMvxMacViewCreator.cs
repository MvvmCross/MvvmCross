// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Mac.Views
{
    public interface IMvxMacViewCreator
    {
        IMvxMacView CreateView(MvxViewModelRequest request);

        IMvxMacView CreateView(IMvxViewModel viewModel);

        IMvxMacView CreateViewOfType([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type viewType, MvxViewModelRequest request);
    }
}
