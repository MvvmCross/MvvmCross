// MvxViewExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.ExtensionMethods
{
    public static class MvxViewExtensionMethods
    {
        public static void OnViewCreate<TViewModel>(this IMvxView<TViewModel> view, Func<TViewModel> viewModelLoader)
            where TViewModel : class, IMvxViewModel
        {
            if (view.ViewModel != null)
                return;

            var viewModel = viewModelLoader();
            viewModel.RegisterView(view);
            view.ViewModel = viewModel;
        }

        public static void OnViewNewIntent<TViewModel>(this IMvxView<TViewModel> view, Func<TViewModel> viewModelLoader)
            where TViewModel : class, IMvxViewModel
        {
            var newViewModel = viewModelLoader();
            view.ReplaceViewModel(newViewModel);
        }

        public static void OnViewDestroy<TViewModel>(this IMvxView<TViewModel> view)
            where TViewModel : class, IMvxViewModel
        {
            if (view.ViewModel != null)
                view.ViewModel.UnRegisterView(view);
        }

        private static void ReplaceViewModel<T>(this IMvxView<T> view, T viewModel)
            where T : class, IMvxViewModel
        {
            if (view.ViewModel == viewModel)
                return;

            if (view.ViewModel != null)
                view.TryUnregisterView();

            view.TryRegisterView();
        }

        private static bool TryRegisterView<T>(this IMvxView<T> view)
            where T : class, IMvxViewModel
        {
            if (view.ViewModel == null)
                return false;
            view.ViewModel.RegisterView(view);
            return true;
        }

        private static bool TryUnregisterView<T>(this IMvxView<T> view)
            where T : class, IMvxViewModel
        {
            if (view.ViewModel == null)
                return false;
            view.ViewModel.UnRegisterView(view);
            return true;
        }

#if NETFX_CORE
        public static PropertyInfo RecursiveGetDeclaredProperty(this TypeInfo type, string name)
        {
            var candidate = type.GetDeclaredProperty(name);
            if (candidate != null)
                return candidate;

            var baseType = type.BaseType;
            if (baseType != null)
                return RecursiveGetDeclaredProperty(baseType.GetTypeInfo(), name);

            return null;
        }
#endif

        public static IMvxViewModel ReflectionGetViewModel(this IMvxView view)
        {
            if (view == null)
                return null;

#if NETFX_CORE
            var propertyInfo = view.GetType().GetTypeInfo().RecursiveGetDeclaredProperty("ViewModel");

            if (propertyInfo == null)
                return null;

            return (IMvxViewModel)propertyInfo.GetValue(view, new object[] { });
#else
            var propertyInfo = view.GetType().GetProperty("ViewModel");

            if (propertyInfo == null)
                return null;

            return (IMvxViewModel) propertyInfo.GetGetMethod().Invoke(view, new object[] {});
#endif
        }
    }
}