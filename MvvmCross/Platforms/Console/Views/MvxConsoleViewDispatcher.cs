// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using MvvmCross.Base;
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

    public async Task<bool> ShowViewModel(MvxViewModelRequest request)
    {
        if (Mvx.IoCProvider?.TryResolve(out IMvxConsoleNavigation? navigation) == true && navigation != null)
        {
            await ExecuteOnMainThreadAsync(() => navigation.Show(request));
            return true;
        }

        return false;
    }

    public async Task<bool> ChangePresentation(MvxPresentationHint hint)
    {
        if (Mvx.IoCProvider?.TryResolve(out IMvxConsoleNavigation? navigation) == true && navigation != null)
        {
            await ExecuteOnMainThreadAsync(() => navigation.ChangePresentation(hint));
            return true;
        }

        return false;
    }
}
