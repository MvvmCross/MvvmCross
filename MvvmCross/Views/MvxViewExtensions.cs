// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Extensions.Logging;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.ViewModels;

namespace MvvmCross.Views
{
#nullable enable
    public static class MvxViewExtensions
    {
        public static void OnViewCreate(this IMvxView view, Func<IMvxViewModel> viewModelLoader)
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
                MvxLogHost.Default?.Log(LogLevel.Warning, "ViewModel not loaded for view {0}", view.GetType().Name);
                return;
            }

            view.ViewModel = viewModel;
        }

        public static void OnViewNewIntent(this IMvxView view, Func<IMvxViewModel> viewModelLoader)
        {
            MvxLogHost.Default?.Log(LogLevel.Warning,
                "OnViewNewIntent isn't well understood or tested inside MvvmCross - it's not really a cross-platform concept.");
            throw new MvxException("OnViewNewIntent is not implemented");
        }

        public static void OnViewDestroy(this IMvxView view)
        {
            // nothing needed currently
        }

        public static Type? FindAssociatedViewModelTypeOrNull(this IMvxView view)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));

            IMvxViewModelTypeFinder associatedTypeFinder;
            if (!Mvx.IoCProvider.TryResolve(out associatedTypeFinder))
            {
                MvxLogHost.Default?.Log(LogLevel.Trace,
                    "No view model type finder available - assuming we are looking for a splash screen - returning null");
                return typeof(MvxNullViewModel);
            }

            return associatedTypeFinder.FindTypeOrNull(view.GetType());
        }

        public static IMvxViewModel? ReflectionGetViewModel(this IMvxView view)
        {
            var propertyInfo = view?.GetType().GetProperty("ViewModel");

            return propertyInfo?.GetGetMethod().Invoke(view, Array.Empty<object>()) as IMvxViewModel;
        }

        public static IMvxBundle CreateSaveStateBundle(this IMvxView view)
        {
            var viewModel = view.ViewModel;
            if (viewModel == null)
                return new MvxBundle();

            return viewModel.SaveStateBundle();
        }
    }
#nullable restore
}
