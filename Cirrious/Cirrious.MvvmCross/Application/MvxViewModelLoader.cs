#region Copyright
// <copyright file="MvxViewModelLoader.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

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
          , IMvxServiceConsumer<IMvxViewModelLocatorFinder>
    {
        #region IMvxViewModelLoader Members

        public IMvxViewModel LoadViewModel(MvxShowViewModelRequest request)
        {
            if (request.ViewModelType == typeof(MvxNullViewModel))
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