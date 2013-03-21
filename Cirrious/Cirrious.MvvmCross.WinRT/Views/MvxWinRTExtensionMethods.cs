// MvxWinRTExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.WinRT.Views
{
    public static class MvxWinRTExtensionMethods
    {
        public static void OnViewCreate(this IMvxWinRTView winRTView, MvxViewModelRequest viewModelRequest, IMvxBundle bundle)
        {
            winRTView.OnViewCreate(() => { return winRTView.LoadViewModel(viewModelRequest, bundle); });
        }

        private static IMvxViewModel LoadViewModel(this IMvxWinRTView winRTView,
                                                   MvxViewModelRequest viewModelRequest,
                                                   IMvxBundle bundle)
        {
            if (viewModelRequest.ClearTop)
            {
#warning TODO - BackStack not cleared for WinRT
                //phoneView.ClearBackStack();
            }

            var loaderService = Mvx.Resolve<IMvxViewModelLoader>();
            var viewModel = loaderService.LoadViewModel(viewModelRequest, bundle);

            return viewModel;
        }

        public static void OnViewCreate(this IMvxWinRTView winRTView, Func<IMvxViewModel> viewModelLoader)
        {
            if (winRTView.ViewModel != null)
                return;

            var viewModel = viewModelLoader();
            winRTView.ViewModel = viewModel;
        }

        public static void OnViewDestroy(this IMvxWinRTView winRTView)
        {
            // nothing to do currently
        }
    }
}