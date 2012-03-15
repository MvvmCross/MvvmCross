#region Copyright
// <copyright file="MvxWinRTExtensionMethods.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
using System;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
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

        private static IMvxViewModel LoadViewModel(this IMvxWinRTView winRTView, MvxShowViewModelRequest viewModelRequest)
        {
            if (viewModelRequest.ClearTop)
            {
#warning TODO!
                //phoneView.ClearBackStack();
            }

            var loaderService = winRTView.GetService<IMvxViewModelLoader>();
            var viewModel = loaderService.LoadViewModel(viewModelRequest);

            return (IMvxViewModel)viewModel;
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