// MvxViewModelLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using System;

namespace Cirrious.MvvmCross.ViewModels
{
    public class MvxViewModelLoader
        : IMvxViewModelLoader
    {
        private IMvxViewModelLocatorCollection _locatorCollection;

        protected IMvxViewModelLocatorCollection LocatorCollection
        {
            get
            {
                _locatorCollection = _locatorCollection ?? Mvx.Resolve<IMvxViewModelLocatorCollection>();
                return _locatorCollection;
            }
        }

        public IMvxViewModel LoadViewModel(MvxViewModelRequest request, IMvxBundle savedState)
        {
            if (request.ViewModelType == typeof(MvxNullViewModel))
            {
                return new MvxNullViewModel();
            }

            var viewModelLocator = FindViewModelLocator(request);

            return LoadViewModel(request, savedState, viewModelLocator);
        }

        private IMvxViewModel LoadViewModel(MvxViewModelRequest request, IMvxBundle savedState,
            IMvxViewModelLocator viewModelLocator)
        {
            IMvxViewModel viewModel = null;
            var parameterValues = new MvxBundle(request.ParameterValues);
            try
            {
                if (!viewModelLocator.TryLoad(request.ViewModelType, parameterValues, savedState, out viewModel))
                {
                    throw new MvxException(
                        string.Format("Failed to construct and initialize ViewModel for type {0} from locator {1} - check MvxTrace for more information",
                            request.ViewModelType, viewModelLocator.GetType().Name));
                }
            }
            catch (Exception ex)
            {
                throw new MvxException(ex,
                    string.Format("Failed to construct and initialize ViewModel for type {0} from locator {1}",
                        request.ViewModelType, viewModelLocator.GetType().Name));
            }

            viewModel.RequestedBy = request.RequestedBy;
            return viewModel;
        }

        private IMvxViewModelLocator FindViewModelLocator(MvxViewModelRequest request)
        {
            var viewModelLocator = LocatorCollection.FindViewModelLocator(request);

            if (viewModelLocator == null)
            {
                throw new MvxException("Sorry - somehow there's no viewmodel locator registered for {0}",
                    request.ViewModelType);
            }

            return viewModelLocator;
        }
    }
}