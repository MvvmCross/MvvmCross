// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Base;
using MvvmCross.Hosting;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Console.Views;

public class MvxConsoleViewDispatcher
    : MvxMainThreadAsyncDispatcher
        , IMvxViewDispatcher
{
    public override bool IsOnMainThread => throw new NotImplementedException();

    public override bool RequestMainThreadAction(Action action, bool maskExceptions = true)
    {
        ExceptionMaskedAction(action, maskExceptions);
        return true;
    }

    [RequiresUnreferencedCode("Getting presentation attribute action uses type hierarchy checks and may call GetPresentationAttribute/CreatePresentationAttribute which require unreferenced code.")]
    public async Task<bool> ShowViewModel(MvxViewModelRequest request)
    {
        var navigation = MvxHost.Current?.Services.GetService<IMvxConsoleNavigation>();
        if (navigation != null)
        {
            await ExecuteOnMainThreadAsync(() => navigation.Show(request));
            return true;
        }

        return false;
    }

    [RequiresUnreferencedCode("Getting presentation attribute action uses type hierarchy checks and may call GetPresentationAttribute/CreatePresentationAttribute which require unreferenced code.")]
    public async Task<bool> ChangePresentation(MvxPresentationHint hint)
    {
        var navigation = MvxHost.Current?.Services.GetService<IMvxConsoleNavigation>();
        if (navigation != null)
        {
            await ExecuteOnMainThreadAsync(() => navigation.ChangePresentation(hint));
            return true;
        }

        return false;
    }
}
