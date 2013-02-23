// MvxViewExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;

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
                MvxTrace.Trace(MvxTraceLevel.Warning, "ViewModel not loaded for view {0}", view.GetType().Name);
                return;
            }

            viewModel.RegisterView(view);
            view.ViewModel = viewModel;
        }


        public static void OnViewNewIntent(this IMvxView view, Func<IMvxViewModel> viewModelLoader)
        {
            var newViewModel = viewModelLoader();
            view.ReplaceViewModel(newViewModel);
        }

        public static void OnViewDestroy(this IMvxView view)
        {
            if (view.ViewModel != null)
                view.ViewModel.UnRegisterView(view);
        }

        // Note that ReplaceViewModel is only really used for Android currently
        // For OnNewIntent - and not sure this is really that useful
        private static void ReplaceViewModel(this IMvxView view, IMvxViewModel viewModel)
        {
            if (view.ViewModel == viewModel)
                return;

            if (view.ViewModel != null)
                view.TryUnregisterView();

            view.TryRegisterView();
        }

        private static bool TryRegisterView(this IMvxView view)
        {
            if (view.ViewModel == null)
                return false;
            view.ViewModel.RegisterView(view);
            return true;
        }

        private static bool TryUnregisterView(this IMvxView view)
        {
            if (view.ViewModel == null)
                return false;
            view.ViewModel.UnRegisterView(view);
            return true;
        }

        public static Type ReflectionGetViewModelType(this IMvxView view)
        {
            if (view == null)
                return null;

            var propertyInfo = view.GetType().GetProperty("ViewModel");

            if (propertyInfo == null)
                return null;

            return propertyInfo.PropertyType;
        }

        public static IMvxViewModel ReflectionGetViewModel(this IMvxView view)
        {
            if (view == null)
                return null;

            var propertyInfo = view.GetType().GetProperty("ViewModel");

            if (propertyInfo == null)
                return null;

            return (IMvxViewModel) propertyInfo.GetGetMethod().Invoke(view, new object[] {});
        }
    }
}