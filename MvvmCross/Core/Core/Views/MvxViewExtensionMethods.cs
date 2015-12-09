// MvxViewExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using System;

namespace Cirrious.MvvmCross.Views
{
    public static class MvxViewExtensionMethods
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
                MvxTrace.Warning("ViewModel not loaded for view {0}", view.GetType().Name);
                return;
            }

            view.ViewModel = viewModel;
        }

        public static void OnViewNewIntent(this IMvxView view, Func<IMvxViewModel> viewModelLoader)
        {
            MvxTrace.Warning(
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
            if (!Mvx.TryResolve(out associatedTypeFinder))
            {
                MvxTrace.Trace(
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