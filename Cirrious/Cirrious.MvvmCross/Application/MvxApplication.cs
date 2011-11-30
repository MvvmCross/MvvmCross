#region Copyright

// <copyright file="MvxApplication.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Core;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Application;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Services;
using Cirrious.MvvmCross.Interfaces.ViewModel;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Application
{
    public abstract class MvxApplication
        : MvxSingleton<MvxApplication>
          , IMvxApplicationTitle
          , IMvxViewModelLocatorFinder
          , IMvxViewModelLocatorStore
    {
        private readonly ViewModelLocatorLookup _lookup = new ViewModelLocatorLookup();

        #region IMvxApplicationTitle Members

        public string Title { get; set; }

        #endregion

        #region IMvxViewModelLocatorFinder Members

        public IMvxViewModelLocator FindLocator(MvxShowViewModelRequest request)
        {
            return _lookup.Find(request);
        }

        #endregion

        #region IMvxViewModelLocatorStore Members

        public void AddLocators(IEnumerable<IMvxViewModelLocator> locators)
        {
            foreach (var locator in locators)
            {
                AddLocator(locator);
            }
        }

        public void AddLocator(IMvxViewModelLocator locator)
        {
            _lookup.Add(locator);
        }

        #endregion

        #region Nested type: ViewModelLocatorLookup

        private class ViewModelLocatorLookup
            : Dictionary<Type, IMvxViewModelLocator>
              , IMvxServiceConsumer<IMvxViewModelLocatorAnalyser>
        {
            public IMvxViewModelLocator Find(MvxShowViewModelRequest request)
            {
                IMvxViewModelLocator toReturn;
                if (!TryGetValue(request.ViewModelType, out toReturn))
                    throw new MvxException("No ViewModelLocator registered for " + request.ViewModelType);
                return this[request.ViewModelType];
            }

            public void Add(IMvxViewModelLocator locator)
            {
                var analyser = this.GetService();
                var methods = analyser.GenerateLocatorMethods(locator.GetType());
                foreach (var method in methods)
                {
                    this[method.ReturnType] = locator;
                }
            }
        }

        #endregion
    }
}