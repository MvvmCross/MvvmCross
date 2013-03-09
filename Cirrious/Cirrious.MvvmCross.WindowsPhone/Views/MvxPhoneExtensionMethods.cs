// MvxPhoneExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.WindowsPhone.Interfaces;

namespace Cirrious.MvvmCross.WindowsPhone.Views
{
    public static class MvxPhoneExtensionMethods
    {
        public static void OnViewCreate(this IMvxWindowsPhoneView phoneView, Uri navigationUri)
        {
            phoneView.OnViewCreate(() => { return phoneView.LoadViewModel(navigationUri); });
        }

        private static IMvxViewModel LoadViewModel(this IMvxWindowsPhoneView phoneView,
                                                   Uri navigationUri)
        {
            var translatorService = Mvx.Resolve<IMvxWindowsPhoneViewModelRequestTranslator>();
            var viewModelRequest = translatorService.GetRequestFromXamlUri(navigationUri);

            if (viewModelRequest.ClearTop)
            {
                phoneView.ClearBackStack();
            }

#warning Nee
            var loaderService = Mvx.Resolve<IMvxViewModelLoader>();
            var viewModel = loaderService.LoadViewModel(viewModelRequest, null);

            return viewModel;
        }
    }
}