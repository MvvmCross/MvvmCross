// MvxViewModelLoader.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.ViewModels
{
    using System;

    using MvvmCross.Platform;
    using MvvmCross.Platform.Exceptions;

    public class MvxViewModelLoader
        : IMvxViewModelLoader
    {
        private IMvxViewModelLocatorCollection _locatorCollection;

        protected IMvxViewModelLocatorCollection LocatorCollection
        {
            get
            {
                this._locatorCollection = this._locatorCollection ?? Mvx.Resolve<IMvxViewModelLocatorCollection>();
                return this._locatorCollection;
            }
        }

        // Reload should be used to re-run cached ViewModels lifecycle if required.
        public IMvxViewModel ReloadViewModel(IMvxViewModel viewModel, MvxViewModelRequest request, IMvxBundle savedState)
        {
            var viewModelLocator = this.FindViewModelLocator(request);
            return this.ReloadViewModel(viewModel, request, savedState, viewModelLocator);
        }

        private IMvxViewModel ReloadViewModel(IMvxViewModel viewModel, MvxViewModelRequest request, IMvxBundle savedState,
                                    IMvxViewModelLocator viewModelLocator)
        {
            if (viewModelLocator == null)
            {
                throw new MvxException("Received view model is null, view model reload failed. ", request.ViewModelType);
            }

            var parameterValues = new MvxBundle(request.ParameterValues);
            try
            {
                viewModel = viewModelLocator.Reload(viewModel, parameterValues, savedState);
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap(
                    "Failed to reload a previously created created ViewModel for type {0} from locator {1} - check InnerException for more information",
                    request.ViewModelType, viewModelLocator.GetType().Name);
            }
            viewModel.RequestedBy = request.RequestedBy;
            return viewModel;
        }

        public IMvxViewModel LoadViewModel(MvxViewModelRequest request, IMvxBundle savedState)
        {
            if (request.ViewModelType == typeof(MvxNullViewModel))
            {
                return new MvxNullViewModel();
            }

            var viewModelLocator = this.FindViewModelLocator(request);

            return this.LoadViewModel(request, savedState, viewModelLocator);
        }

        private IMvxViewModel LoadViewModel(MvxViewModelRequest request, IMvxBundle savedState,
                                            IMvxViewModelLocator viewModelLocator)
        {
            IMvxViewModel viewModel = null;
            var parameterValues = new MvxBundle(request.ParameterValues);
            try
            {
                viewModel = viewModelLocator.Load(request.ViewModelType, parameterValues, savedState);
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap(
                    "Failed to construct and initialize ViewModel for type {0} from locator {1} - check InnerException for more information",
                    request.ViewModelType, viewModelLocator.GetType().Name);
            }
            viewModel.RequestedBy = request.RequestedBy;
            return viewModel;
        }

        private IMvxViewModelLocator FindViewModelLocator(MvxViewModelRequest request)
        {
            var viewModelLocator = this.LocatorCollection.FindViewModelLocator(request);

            if (viewModelLocator == null)
            {
                throw new MvxException("Sorry - somehow there's no viewmodel locator registered for {0}",
                                       request.ViewModelType);
            }

            return viewModelLocator;
        }
    }
}