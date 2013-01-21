// MvxWinRTExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.WinRT.Interfaces;

namespace Cirrious.MvvmCross.WinRT.Views
{
    public static class MvxWinRTExtensionMethods
    {
        public static void OnViewCreate(this IMvxWinRTView winRTView, MvxShowViewModelRequest viewModelRequest)
        {
            winRTView.OnViewCreate(() => { return winRTView.LoadViewModel(viewModelRequest); });
        }

        private static IMvxViewModel LoadViewModel(this IMvxWinRTView winRTView,
                                                   MvxShowViewModelRequest viewModelRequest)
        {
            if (viewModelRequest.ClearTop)
            {
#warning TODO - BackStack not cleared for WinRT
                //phoneView.ClearBackStack();
            }

            var loaderService = winRTView.GetService<IMvxViewModelLoader>();
            var viewModel = loaderService.LoadViewModel(viewModelRequest);

            return viewModel;
        }

        public static void OnViewCreate(this IMvxWinRTView winRTView, Func<IMvxViewModel> viewModelLoader)
        {
            if (winRTView.ViewModel != null)
                return;

            var viewModel = viewModelLoader();
            viewModel.RegisterView(winRTView);
            winRTView.ViewModel = viewModel;
        }

        public static void OnViewDestroy(this IMvxWinRTView winRTView)
        {
            if (winRTView.ViewModel != null)
                winRTView.ViewModel.UnRegisterView(winRTView);
        }
    }
}