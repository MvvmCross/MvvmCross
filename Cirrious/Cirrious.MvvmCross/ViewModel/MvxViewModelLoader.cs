#region Copyright
// <copyright file="MvxViewModelLoader.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Application;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModel;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.ViewModel
{
    public class MvxViewModelLoader
        : IMvxViewModelLoader
        , IMvxServiceConsumer<IMvxViewModelLocatorFinder>
    {
        #region IMvxViewModelLoader Members

        public IMvxViewModel LoadModel<T>(MvxShowViewModelRequest request) 
            where T : IMvxViewModel
        {
            var viewModelLocatorFinder = this.GetService<IMvxViewModelLocatorFinder>();
            var viewModelLocator = viewModelLocatorFinder.FindLocator(request);

            if (viewModelLocator == null)
                throw new MvxException("Sorry - somehow there's no viewmodel locator wired up for {0} looking for {1}",
                                       GetType().Name, typeof (T).Name);

            if (viewModelLocator.ViewModelType != typeof(T))
                throw new MvxException(
                    "Sorry - somehow view {0} has been wired up to viewmodel type {1} but received a request to show viewmodel type {2}",
                    GetType().Name, typeof(T).Name, viewModelLocator.ViewModelType.Name);


            IMvxViewModel model = null;
            if (!viewModelLocator.TryLoad(request.ViewModelAction.ActionName, request.ParameterValues, out model))
                throw new MvxException(
                    "Failed to load ViewModel for type {0} action {1}",
                    typeof(T).Name, request.ViewModelAction.ActionName);

            return model;
        }

        #endregion
    }
}