// MvxStoreExtensionMethods.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.WindowsStore.Views
{
    using System;

    using MvvmCross.Core.ViewModels;
    using MvvmCross.Platform;

    public static class MvxStoreExtensionMethods
    {
        public static void OnViewCreate(this IMvxStoreView storeView, MvxViewModelRequest viewModelRequest, Func<IMvxBundle> bundleLoader)
        {
            storeView.OnViewCreate(() => { return storeView.LoadViewModel(viewModelRequest, bundleLoader()); });
        }

        private static IMvxViewModel LoadViewModel(this IMvxStoreView storeView,
                                                   MvxViewModelRequest viewModelRequest,
                                                   IMvxBundle bundle)
        {
#warning ClearingBackStack disabled for now
            //            if (viewModelRequest.ClearTop)
            //            {
            //#warning TODO - BackStack not cleared for WinRT
            //phoneView.ClearBackStack();
            //            }

            var loaderService = Mvx.Resolve<IMvxViewModelLoader>();
            var viewModel = loaderService.LoadViewModel(viewModelRequest, bundle);

            return viewModel;
        }

        public static void OnViewCreate(this IMvxStoreView storeView, Func<IMvxViewModel> viewModelLoader)
        {
            if (storeView.ViewModel != null)
                return;

            var viewModel = viewModelLoader();
            storeView.ViewModel = viewModel;
        }

        public static void OnViewDestroy(this IMvxStoreView storeView)
        {
            // nothing to do currently
        }
    }
}