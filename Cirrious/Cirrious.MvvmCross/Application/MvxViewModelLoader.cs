// MvxViewModelLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Application;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Application
{
    public class MvxViewModelLoader
        : IMvxViewModelLoader
        , IMvxServiceConsumer
    {
        #region IMvxViewModelLoader Members

        public IMvxViewModel LoadViewModel(MvxShowViewModelRequest request)
        {
            if (request.ViewModelType == typeof (MvxNullViewModel))
                return new MvxNullViewModel();

			var viewModelLocatorFinder = this.GetService<IMvxViewModelLocatorFinder>();
			var viewModelLocator = viewModelLocatorFinder.FindLocator(request);

            if (viewModelLocator == null)
                throw new MvxException("Sorry - somehow there's no viewmodel locator registered for {0}",
                                       request.ViewModelType);

            IMvxViewModel model = null;
            if (!viewModelLocator.TryLoad(request.ViewModelType, request.ParameterValues, out model))
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