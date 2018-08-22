// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Android.Views
{
    public static class MvxFragmentExtensions
    {
        public static Type FindAssociatedViewModelType(this IMvxFragmentView fragmentView, Type fragmentActivityParentType)
        {
            var viewModelType = fragmentView.FindAssociatedViewModelTypeOrNull();

            var type = fragmentView.GetType();

            if (viewModelType == null)
            {
                if (!type.HasBasePresentationAttribute())
                    throw new InvalidOperationException($"Your fragment is not generic and it does not have {nameof(MvxFragmentPresentationAttribute)} attribute set!");

                var cacheableFragmentAttribute = type.GetBasePresentationAttribute();
                if (cacheableFragmentAttribute.ViewModelType == null)
                    throw new InvalidOperationException($"Your fragment is not generic and it does not use {nameof(MvxFragmentPresentationAttribute)} with ViewModel Type constructor.");

                viewModelType = cacheableFragmentAttribute.ViewModelType;
            }

            return viewModelType;
        }

        public static IMvxViewModel LoadViewModel(this IMvxFragmentView fragmentView, IMvxBundle savedState, Type fragmentParentActivityType,
            MvxViewModelRequest request = null)
        {
            var viewModelType = fragmentView.FindAssociatedViewModelType(fragmentParentActivityType);
            if (viewModelType == typeof(MvxNullViewModel))
                return new MvxNullViewModel();

            if (viewModelType == null
                || viewModelType == typeof(IMvxViewModel))
            {
                MvxLog.Instance.Trace("No ViewModel class specified for {0} in LoadViewModel",
                    fragmentView.GetType().Name);
            }

            if (request == null)
                request = MvxViewModelRequest.GetDefaultRequest(viewModelType);

            var viewModelCache = Mvx.IoCProvider.Resolve<IMvxChildViewModelCache>();
            if (viewModelCache.Exists(viewModelType))
            {
                var viewModelCached = viewModelCache.Get(viewModelType);
                viewModelCache.Remove(viewModelType);
                return viewModelCached;
            }

            var loaderService = Mvx.IoCProvider.Resolve<IMvxViewModelLoader>();
            var viewModel = loaderService.LoadViewModel(request, savedState);

            return viewModel;
        }

        public static void RunViewModelLifecycle(IMvxViewModel viewModel, IMvxBundle savedState,
            MvxViewModelRequest request)
        {
            try
            {
                if (request != null)
                {
                    var parameterValues = new MvxBundle(request.ParameterValues);
                    viewModel.CallBundleMethods("Init", parameterValues);
                }
                if (savedState != null)
                {
                    viewModel.CallBundleMethods("ReloadState", savedState);
                }
                viewModel.Start();
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap("Problem running viewModel lifecycle of type {0}", viewModel.GetType().Name);
            }
        }
    }
}
