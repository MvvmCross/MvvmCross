// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MvvmCross.Base;
using MvvmCross.ViewModels;

namespace MvvmCross.Views
{
#nullable enable
    public interface IMvxViewDispatcher : IMvxMainThreadAsyncDispatcher, IMvxMainThreadDispatcher
    {
        [RequiresUnreferencedCode("Getting presentation attribute action uses type hierarchy checks and may call GetPresentationAttribute/CreatePresentationAttribute which require unreferenced code.")]
        Task<bool> ShowViewModel(MvxViewModelRequest request);

        [RequiresUnreferencedCode("Getting presentation attribute action uses type hierarchy checks and may call GetPresentationAttribute/CreatePresentationAttribute which require unreferenced code.")]
        Task<bool> ChangePresentation(MvxPresentationHint hint);
    }
#nullable restore
}
