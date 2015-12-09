// MvxPhoneExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using System;

namespace Cirrious.MvvmCross.WindowsPhone.Views
{
    public static class MvxPhoneExtensionMethods
    {
        public static void OnViewCreate(this IMvxPhoneView phoneView, Uri navigationUri, IMvxBundle savedStateBundle)
        {
            phoneView.OnViewCreate(() => { return phoneView.LoadViewModel(navigationUri, savedStateBundle); });
        }

        private static IMvxViewModel LoadViewModel(this IMvxPhoneView phoneView,
                                                   Uri navigationUri,
                                                   IMvxBundle savedStateBundle)
        {
            var translatorService = Mvx.Resolve<IMvxPhoneViewModelRequestTranslator>();
            var viewModelRequest = translatorService.GetRequestFromXamlUri(navigationUri);

#warning ClearingBackStack disabled for now
            //if (viewModelRequest.ClearTop)
            //{
            //    phoneView.ClearBackStack();
            //}

            var loaderService = Mvx.Resolve<IMvxViewModelLoader>();
            var viewModel = loaderService.LoadViewModel(viewModelRequest, savedStateBundle);

            return viewModel;
        }
    }
}