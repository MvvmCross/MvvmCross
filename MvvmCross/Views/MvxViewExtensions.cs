// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.ViewModels;

namespace MvvmCross.Views
{
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
                MvxLog.Instance.Warn("ViewModel not loaded for view {0}", view.GetType().Name);
                return;
            }

            view.ViewModel = viewModel;
        }

        public static void OnViewNewIntent(this IMvxView view, Func<IMvxViewModel> viewModelLoader)
        {
            MvxLog.Instance.Warn(
                "OnViewNewIntent isn't well understood or tested inside MvvmCross - it's not really a cross-platform concept.");
            throw new MvxException("OnViewNewIntent is not implemented");
        }

        public static void OnViewDestroy(this IMvxView view)
        {
            // nothing needed currently
        }

        public static Type FindAssociatedViewModelTypeOrNull(this IMvxView view)
        {
            if (view == null)
                return null;

            IMvxViewModelTypeFinder associatedTypeFinder;
            if (!Mvx.IoCProvider.TryResolve(out associatedTypeFinder))
            {
                MvxLog.Instance.Trace(
                    "No view model type finder available - assuming we are looking for a splash screen - returning null");
                return typeof(MvxNullViewModel);
            }

            return associatedTypeFinder.FindTypeOrNull(view.GetType());
        }

        public static IMvxViewModel ReflectionGetViewModel(this IMvxView view)
        {
            var propertyInfo = view?.GetType().GetProperty("ViewModel");

            return (IMvxViewModel) propertyInfo?.GetGetMethod().Invoke(view, new object[] { });
        }

        public static IMvxBundle CreateSaveStateBundle(this IMvxView view)
        {
            var viewModel = view.ViewModel;
            if (viewModel == null)
                return new MvxBundle();

            return viewModel.SaveStateBundle();
        }
    }
}
