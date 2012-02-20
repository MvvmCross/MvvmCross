using System;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.ExtensionMethods
{
    public static class MvxViewExtensionMethods
    {
        public static void OnViewCreate<TViewModel>(this IMvxTrackedView<TViewModel> view, Func<TViewModel> viewModelLoader)
            where TViewModel : class, IMvxViewModel
        {
            if (view.ViewModel != null)
                return;

            var viewModel = viewModelLoader();
            viewModel.RegisterView(view);
            view.ViewModel = (TViewModel)viewModel;
        }

        public static void OnViewDestroy<TViewModel>(this IMvxTrackedView<TViewModel> view)
            where TViewModel : class, IMvxViewModel
        {
            if (view.ViewModel != null)
                view.ViewModel.UnRegisterView(view);
        }

        public static void FixupTracking<T>(this IMvxTrackedView<T> view, T viewModel, Action setViewModelCallback)
            where T : class, IMvxViewModel
        {
            if (view.ViewModel == viewModel)
                return;

            if (view.ViewModel != null)
                view.TryUnregisterView();

            setViewModelCallback();
            view.TryRegisterView();
        }

        private static bool TryRegisterView<T>(this IMvxTrackedView<T> view)
            where T : class, IMvxViewModel
        {
            if (view.ViewModel == null)
                return false;
            view.ViewModel.RegisterView(view);
            return true;
        }

        private static bool TryUnregisterView<T>(this IMvxTrackedView<T> view)
            where T : class, IMvxViewModel
        {
            if (view.ViewModel == null)
                return false;
            view.ViewModel.UnRegisterView(view);
            return true;
        }
    }
}