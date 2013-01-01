// MvxPhoneExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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

        private static TViewModel LoadViewModel<TViewModel>(this IMvxWindowsPhoneView<TViewModel> phoneView,
                                                            Uri navigationUri)
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

            return (TViewModel) viewModel;
        }
    }
}