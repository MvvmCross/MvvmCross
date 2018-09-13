// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.Core;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Mac.Views
{
    public static class MvxViewControllerExtensions
    {
        public static void OnViewCreate(this IMvxMacView macView)
        {
            //var view = touchView as IMvxView<TViewModel>;
            macView.OnViewCreate(() => { return macView.LoadViewModel(); });
        }

        private static IMvxViewModel LoadViewModel(this IMvxMacView macView)
        {
            if (macView.Request == null)
            {
                MvxLog.Instance.Trace(
                    "Request is null - assuming this is a TabBar type situation where ViewDidLoad is called during construction... patching the request now - but watch out for problems with virtual calls during construction");
                macView.Request = Mvx.IoCProvider.Resolve<IMvxCurrentRequest>().CurrentRequest;
            }

            var instanceRequest = macView.Request as MvxViewModelInstanceRequest;
            if (instanceRequest != null)
            {
                return instanceRequest.ViewModelInstance;
            }

            var loader = Mvx.IoCProvider.Resolve<IMvxViewModelLoader>();
            var viewModel = loader.LoadViewModel(macView.Request, null /* no saved state on iOS currently */);
            if (viewModel == null)
                throw new MvxException("ViewModel not loaded for " + macView.Request.ViewModelType);
            return viewModel;
        }

        public static IMvxMacView CreateViewControllerFor<TTargetViewModel>(this IMvxMacView view,
                                                                            object parameterObject)
            where TTargetViewModel : class, IMvxViewModel
        {
            return
                view.CreateViewControllerFor<TTargetViewModel>(parameterObject == null
                                                                   ? null
                                                                   : parameterObject.ToSimplePropertyDictionary());
        }

#warning TODO - could this move down to IMvxView level?

        public static IMvxMacView CreateViewControllerFor<TTargetViewModel>(
            this IMvxMacView view,
            IDictionary<string, string> parameterValues = null)
            where TTargetViewModel : class, IMvxViewModel
        {
            var parameterBundle = new MvxBundle(parameterValues);
            var request = new MvxViewModelRequest<TTargetViewModel>(parameterBundle, null);
            return view.CreateViewControllerFor(request);
        }

        public static IMvxMacView CreateViewControllerFor<TTargetViewModel>(
            this IMvxCanCreateMacView view,
            MvxViewModelRequest request)
            where TTargetViewModel : class, IMvxViewModel
        {
            return Mvx.IoCProvider.Resolve<IMvxMacViewCreator>().CreateView(request);
        }

        public static IMvxMacView CreateViewControllerFor(
            this IMvxCanCreateMacView view,
            MvxViewModelRequest request)
        {
            return Mvx.IoCProvider.Resolve<IMvxMacViewCreator>().CreateView(request);
        }

        public static IMvxMacView CreateViewControllerFor(
            this IMvxCanCreateMacView view, Type viewType,
            MvxViewModelRequest request)
        {
            return Mvx.IoCProvider.Resolve<IMvxMacViewCreator>().CreateViewOfType(viewType, request);
        }

        public static IMvxMacView CreateViewControllerFor(
            this IMvxCanCreateMacView view,
            IMvxViewModel viewModel)
        {
            return Mvx.IoCProvider.Resolve<IMvxMacViewCreator>().CreateView(viewModel);
        }
    }
}
