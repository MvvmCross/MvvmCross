// MvxViewModelLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.IoC;

namespace Cirrious.MvvmCross.ViewModels
{
    public class MvxViewModelLoader
        : IMvxViewModelLoader
    {
        #region IMvxViewModelLoader Members

        public IMvxViewModel LoadViewModel(MvxShowViewModelRequest request, IMvxBundle savedState)
        {
            if (request.ViewModelType == typeof (MvxNullViewModel))
                return new MvxNullViewModel();

            var viewModelLocatorFinder = Mvx.Resolve<IMvxViewModelLocatorFinder>();
            var viewModelLocator = viewModelLocatorFinder.FindLocator(request);

            if (viewModelLocator == null)
                throw new MvxException("Sorry - somehow there's no viewmodel locator registered for {0}",
                                       request.ViewModelType);

            IMvxViewModel model = null;
            var parameterValues = new MvxBundle(request.ParameterValues);
            if (!viewModelLocator.TryLoad(request.ViewModelType, parameterValues, savedState, out model))
                throw new MvxException(
                    "Failed to load ViewModel for type {0} from locator {1}",
                    request.ViewModelType, viewModelLocator.GetType().Name);

            if (model != null)
                model.RequestedBy = request.RequestedBy;

            return model;
        }

        #endregion
    }
}