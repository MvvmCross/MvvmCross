// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MvvmCross.ViewModels;

namespace MvvmCross.Presenters
{
    public interface IMvxViewPresenter
    {
        [RequiresUnreferencedCode("Getting presentation attribute action uses type hierarchy checks and may call GetPresentationAttribute/CreatePresentationAttribute which require unreferenced code.")]
        Task<bool> Show(MvxViewModelRequest request);

        [RequiresUnreferencedCode("Getting presentation attribute action uses type hierarchy checks and may call GetPresentationAttribute/CreatePresentationAttribute which require unreferenced code.")]
        Task<bool> ChangePresentation(MvxPresentationHint hint);

        void AddPresentationHintHandler<THint>(Func<THint, Task<bool>> action) where THint : MvxPresentationHint;

        [RequiresUnreferencedCode("Getting presentation attribute action uses type hierarchy checks and may call GetPresentationAttribute/CreatePresentationAttribute which require unreferenced code.")]
        Task<bool> Close(IMvxViewModel viewModel);
    }
}
