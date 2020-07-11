// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using MvvmCross.Exceptions;
using MvvmCross.Navigation.EventArguments;

namespace MvvmCross.ViewModels
{
    public class MvxViewModelLoader
        : IMvxViewModelLoader
    {
        protected IMvxViewModelLocatorCollection LocatorCollection { get; private set; }

        public MvxViewModelLoader(IMvxViewModelLocatorCollection locatorCollection)
        {
            LocatorCollection = locatorCollection;
        }

        // Reload should be used to re-run cached ViewModels lifecycle if required.
        public ValueTask<IMvxViewModel> ReloadViewModel(IMvxViewModel viewModel, MvxViewModelRequest request, IMvxBundle savedState, IMvxNavigateEventArgs navigationArgs)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var viewModelLocator = FindViewModelLocator(request);

            var parameterValues = new MvxBundle(request.ParameterValues);
            try
            {
                viewModel = viewModelLocator.Reload(viewModel, parameterValues, savedState, navigationArgs);
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap(
                    "Failed to reload a previously created created ViewModel for type {0} from locator {1} - check InnerException for more information",
                    request.ViewModelType, viewModelLocator.GetType().Name);
            }

            return new ValueTask<IMvxViewModel>(viewModel);
        }

        public ValueTask<IMvxViewModel> ReloadViewModel<TParameter>(IMvxViewModel<TParameter> viewModel, TParameter param, MvxViewModelRequest request, IMvxBundle savedState, IMvxNavigateEventArgs navigationArgs)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var viewModelLocator = FindViewModelLocator(request);

            var parameterValues = new MvxBundle(request.ParameterValues);
            try
            {
                viewModel = viewModelLocator.Reload(viewModel, param, parameterValues, savedState, navigationArgs);
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap(
                    "Failed to reload a previously created created ViewModel for type {0} from locator {1} - check InnerException for more information",
                    request.ViewModelType, viewModelLocator.GetType().Name);
            }

            return new ValueTask<IMvxViewModel>(viewModel);
        }

        public ValueTask<IMvxViewModel> LoadViewModel(MvxViewModelRequest request, IMvxBundle savedState, IMvxNavigateEventArgs navigationArgs)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            if (request.ViewModelType == typeof(MvxNullViewModel))
            {
                return new ValueTask<IMvxViewModel>(new MvxNullViewModel());
            }

            var viewModelLocator = FindViewModelLocator(request);

            var parameterValues = new MvxBundle(request.ParameterValues);
            try
            {
                var viewModel = viewModelLocator.Load(request.ViewModelType, parameterValues, savedState, navigationArgs);
                return new ValueTask<IMvxViewModel>(viewModel);
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap(
                    "Failed to construct and initialize ViewModel for type {0} from locator {1} - check InnerException for more information",
                    request.ViewModelType, viewModelLocator.GetType().Name);
            }
        }

        public ValueTask<IMvxViewModel> LoadViewModel<TParameter>(MvxViewModelRequest request, TParameter param, IMvxBundle savedState, IMvxNavigateEventArgs navigationArgs)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            if (request.ViewModelType == typeof(MvxNullViewModel))
            {
                return new ValueTask<IMvxViewModel>(new MvxNullViewModel());
            }

            var viewModelLocator = FindViewModelLocator(request);

            var parameterValues = new MvxBundle(request.ParameterValues);
            try
            {
                var viewModel = viewModelLocator.Load(request.ViewModelType, param, parameterValues, savedState, navigationArgs);

                return new ValueTask<IMvxViewModel>(viewModel);
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap(
                    "Failed to construct and initialize ViewModel for type {0} from locator {1} - check InnerException for more information",
                    request.ViewModelType, viewModelLocator.GetType().Name);
            }
        }

        private IMvxViewModelLocator FindViewModelLocator(MvxViewModelRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var viewModelLocator = LocatorCollection.FindViewModelLocator(request);

            if (viewModelLocator == null)
            {
                throw new MvxException($"Sorry - somehow there's no viewmodel locator registered for {request.ViewModelType}");
            }

            return viewModelLocator;
        }
    }
}
