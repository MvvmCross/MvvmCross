#region Copyright
// <copyright file="MvxTouchViewControllerExtensionMethods.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
using System;
using Cirrious.CrossCore.Interfaces.IoC;


#endregion

using System.Collections.Generic;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Mac.Interfaces;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Mac.ExtensionMethods
{
    public static class MvxMacViewControllerExtensionMethods
    {
		public static void OnViewCreate(this IMvxMacView macView, MvxShowViewModelRequest viewModelRequest)
		{
			macView.OnViewCreate(() => { return macView.LoadViewModel(viewModelRequest); });
		}

		private static IMvxViewModel LoadViewModel(this IMvxMacView macView,
		                                           MvxShowViewModelRequest viewModelRequest)
		{
			if (viewModelRequest.ClearTop)
			{
#warning TODO - BackStack not cleared for Mac
				//phoneView.ClearBackStack();
			}
			
			var loaderService = Mvx.Resolve<IMvxViewModelLoader>();
			var viewModel = loaderService.LoadViewModel(viewModelRequest);
			
			return viewModel;
		}

		public static void OnViewCreate(this IMvxMacView macView, Func<IMvxViewModel> viewModelLoader)
		{
			if (macView.ViewModel != null)
				return;
			
			var viewModel = viewModelLoader();
			viewModel.RegisterView(macView);
			macView.ViewModel = viewModel;
		}
		
		public static void OnViewDestroy(this IMvxMacView winRTView)
		{
			if (winRTView.ViewModel != null)
				winRTView.ViewModel.UnRegisterView(winRTView);
		}

    }
}