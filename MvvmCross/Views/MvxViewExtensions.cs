// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
using Microsoft.Extensions.Logging;
using MvvmCross.Logging;
using MvvmCross.ViewModels;

namespace MvvmCross.Views;

public static class MvxViewExtensions
{
    public static void OnViewCreate(this IMvxView view, Func<IMvxViewModel?> viewModelLoader)
    {
        // note - we check the DataContent before the ViewModel to avoid casting errors
        //       in the case of 'simple' binding code
        if (view.DataContext != null)
            return;

        if (view.ViewModel != null)
            return;

        var viewModel = viewModelLoader();
        if (viewModel == null)
        {
            MvxLogHost.Default?.Log(LogLevel.Warning, "ViewModel not loaded for view {ViewTypeName}", view.GetType().Name);
            return;
        }

        view.ViewModel = viewModel;
    }

    public static void OnViewDestroy(this IMvxView view)
    {
        // nothing needed currently
    }

    public static Type? FindAssociatedViewModelTypeOrNull(this IMvxView view)
    {
        if (view == null)
            throw new ArgumentNullException(nameof(view));

        if (Mvx.IoCProvider?.TryResolve(out IMvxViewModelTypeFinder associatedTypeFinder) == true)
            return associatedTypeFinder.FindTypeOrNull(view.GetType());

        MvxLogHost.Default?.Log(LogLevel.Trace,
            "No view model type finder available - assuming we are looking for a splash screen - returning null");
        return typeof(MvxNullViewModel);
    }

    public static IMvxBundle CreateSaveStateBundle(this IMvxView view)
    {
        var viewModel = view.ViewModel;
        return viewModel == null ? new MvxBundle() : viewModel.SaveStateBundle();
    }
}
