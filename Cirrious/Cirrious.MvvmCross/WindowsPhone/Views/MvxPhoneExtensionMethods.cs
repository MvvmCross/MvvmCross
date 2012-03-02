#region Copyright
// <copyright file="MvxPhoneExtensionMethods.cs" company="Cirrious">
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
using Cirrious.MvvmCross.WindowsPhone.Interfaces;

namespace Cirrious.MvvmCross.WindowsPhone.Views
{
    public static class MvxPhoneExtensionMethods
    {
        public static void OnViewCreate<TViewModel>(this IMvxWindowsPhoneView<TViewModel> phoneView, Uri navigationUri)
            where TViewModel : class, IMvxViewModel
        {
            var view = phoneView as IMvxView<TViewModel>;
            view.OnViewCreate(() => { return phoneView.LoadViewModel(navigationUri); });
        }

        private static TViewModel LoadViewModel<TViewModel>(this IMvxWindowsPhoneView<TViewModel> phoneView, Uri navigationUri)
            where TViewModel : class, IMvxViewModel
        {
            var translatorService = phoneView.GetService<IMvxWindowsPhoneViewModelRequestTranslator>();
            var viewModelRequest = translatorService.GetRequestFromXamlUri(navigationUri);

            if (viewModelRequest.ClearTop)
            {
                phoneView.ClearBackStack();
            }

            var loaderService = phoneView.GetService<IMvxViewModelLoader>();
            var viewModel = loaderService.LoadViewModel(viewModelRequest);

            return (TViewModel)viewModel;
        }
    }
}