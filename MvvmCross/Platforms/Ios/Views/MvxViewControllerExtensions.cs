// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable

using Microsoft.Extensions.Logging;
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
            iosView.OnViewCreate(iosView.LoadViewModel);
        }

        private static IMvxViewModel LoadViewModel(this IMvxIosView iosView)
        {
            if (iosView.Request == null)
            {
                MvxLogHost.Default?.LogTrace(
                    "MvxViewControllerExtensions: LoadViewModelRequest is null - assuming this is a TabBar type situation where ViewDidLoad is called during construction... patching the request now - but watch out for problems with virtual calls during construction");

                if (Mvx.IoCProvider?.TryResolve(out IMvxCurrentRequest? currentRequest) == true &&
                    currentRequest?.CurrentRequest != null)
                {
                    iosView.Request = currentRequest.CurrentRequest;
                }
            }

            if (iosView.Request is MvxViewModelInstanceRequest instanceRequest &&
                instanceRequest.ViewModelInstance != null)
            {
                MvxLogHost.Default?.LogTrace(
                    "MvxViewControllerExtensions: LoadViewModel ({ViewModelType}) instance already set - returning it directly without loading from locator",
                    instanceRequest.ViewModelInstance.GetType().Name);
                return instanceRequest.ViewModelInstance;
            }

            if (iosView.Request != null &&
                Mvx.IoCProvider?.TryResolve(out IMvxViewModelLoader? viewModelLoader) == true &&
                viewModelLoader != null)
            {
                var viewModel = viewModelLoader.LoadViewModel(iosView.Request, null /* no saved state on iOS currently */);
                if (viewModel == null)
                    throw new MvxException($"ViewModel not loaded for {iosView.Request.ViewModelType}");

                MvxLogHost.Default?.LogTrace(
                    "MvxViewControllerExtensions: LoadViewModel loaded ({ViewModelType})",
                    viewModel.GetType().Name);
                return viewModel;
            }

            throw new MvxException("ViewModel not loaded for null Request on {0}", iosView.GetType().Name);
        }
    }
}
