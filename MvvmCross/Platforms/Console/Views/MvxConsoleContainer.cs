// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using Microsoft.Extensions.Logging;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Console.Views;

public class MvxConsoleContainer
    : MvxBaseConsoleContainer
{
    private readonly object _lockObject = new();
    private readonly Stack<MvxViewModelRequest> _navigationStack = new();

    public override Task<bool> Show(MvxViewModelRequest request)
    {
        lock (_lockObject)
        {
            var viewType = GetViewType(request.ViewModelType);
            if (viewType == null)
            {
                throw new MvxException("View Type not found for " + request.ViewModelType);
            }
            var view = Activator.CreateInstance(viewType) as IMvxConsoleView;
            if (Mvx.IoCProvider?.TryResolve(out IMvxViewModelLoader? viewModelLoader) == true)
            {
                IMvxBundle? savedState = null;
                var viewModel = viewModelLoader?.LoadViewModel(request, savedState);
                view?.HackSetViewModel(viewModel);
                if (Mvx.IoCProvider.TryResolve(out IMvxConsoleCurrentView? currentView) && currentView != null)
                    currentView.CurrentView = view;

                _navigationStack.Push(request);
            }
        }

        return Task.FromResult(true);
    }

    public override async Task<bool> ChangePresentation(MvxPresentationHint hint)
    {
        if (await HandlePresentationChange(hint).ConfigureAwait(true)) return true;

        if (hint is MvxClosePresentationHint closeHint)
        {
            return await Close(closeHint.ViewModelToClose).ConfigureAwait(true);
        }

        MvxLogHost.GetLog<MvxConsoleContainer>()?.Log(LogLevel.Trace, "Hint ignored {0}", hint.GetType().Name);
        return false;
    }

    public override Task<bool> Close(IMvxViewModel viewModel)
    {
        if (Mvx.IoCProvider?.TryResolve(out IMvxConsoleCurrentView? currentView) != true)
        {
            MvxLogHost.GetLog<MvxConsoleContainer>()?.Log(LogLevel.Warning, "No current view set. Cannot close it");
            return Task.FromResult(false);
        }

        if (currentView?.CurrentView == null)
        {
            MvxLogHost.GetLog<MvxConsoleContainer>()?.Log(LogLevel.Warning, "Ignoring close for viewmodel - root frame has no current page");
            return Task.FromResult(true);
        }

        if (currentView.CurrentView?.ViewModel != viewModel)
        {
            MvxLogHost.GetLog<MvxConsoleContainer>()?.Log(LogLevel.Warning, "Ignoring close for viewmodel - root frame's current page is not the view for the requested viewmodel");
            return Task.FromResult(true);
        }

        return GoBack();
    }

    public override Task<bool> GoBack()
    {
        lock (_lockObject)
        {
            if (!CanGoBack())
            {
                System.Console.WriteLine("Back not possible");
                return Task.FromResult(true);
            }

            // pop off the current view
            _navigationStack.Pop();

            // prepare to re-push the current view
            var backTo = _navigationStack.Pop();

            // re-display the view
            return Show(backTo);
        }
    }

    public override void RemoveBackEntry()
    {
        throw new NotImplementedException("RemoveBackEntry not supported on console currently");
    }

    public override bool CanGoBack()
    {
        lock (_lockObject)
        {
            return _navigationStack.Count > 1;
        }
    }
}
