// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Ios.Views
{
    public static class MvxViewControllerExtensions
    {
        public static void OnViewCreate(this IMvxIosView iosView)
        {
            //var view = iosView as IMvxView<TViewModel>;
            iosView.OnViewCreate(iosView.LoadViewModel);
        }

        private static IMvxViewModel LoadViewModel(this IMvxIosView iosView)
        {
            if (iosView.Request == null)
            {
                MvxLog.Instance.Trace(
                    "Request is null - assuming this is a TabBar type situation where ViewDidLoad is called during construction... patching the request now - but watch out for problems with virtual calls during construction");
                iosView.Request = Mvx.IoCProvider.Resolve<IMvxCurrentRequest>().CurrentRequest;
            }

            var instanceRequest = iosView.Request as MvxViewModelInstanceRequest;
            if (instanceRequest != null)
            {
                return instanceRequest.ViewModelInstance;
            }

            var loader = Mvx.IoCProvider.Resolve<IMvxViewModelLoader>();
            var viewModel = loader.LoadViewModel(iosView.Request, null /* no saved state on iOS currently */);
            if (viewModel == null)
                throw new MvxException("ViewModel not loaded for " + iosView.Request.ViewModelType);
            return viewModel;
        }
    }
}
