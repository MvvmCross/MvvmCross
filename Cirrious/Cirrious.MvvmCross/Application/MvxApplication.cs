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

        #region Nested type: ViewModelActionLookup

        private class ViewModelActionLookup 
            : Dictionary<string, IMvxViewModelLocator>
              , IMvxServiceConsumer<IMvxViewModelLocatorAnalyser>
        {
            public IMvxViewModelLocator Find(MvxShowViewModelRequest request)
            {
                return this[request.ViewModelAction.ActionName ?? string.Empty];
            }

            public void Add(IMvxViewModelLocator locator)
            {
                var analyser = this.GetService();
                var map = analyser.GenerateLocatorMap(locator.GetType(), locator.ViewModelType);
                foreach (var name in map.Keys)
                {
                    this[name] = locator;
                }
                if (!string.IsNullOrEmpty(locator.DefaultAction))
                    this[string.Empty] = locator;
            }
        }

        #endregion

        #region Nested type: ViewModelLocatorLookup

        private class ViewModelLocatorLookup : Dictionary<string, ViewModelActionLookup>
        {
            public IMvxViewModelLocator Find(MvxShowViewModelRequest request)
            {
                var lookup = GetOrCreateLookup(request.ViewModelAction.ViewModelType);
                return lookup.Find(request);                
            }

            public void Add(IMvxViewModelLocator locator)
            {
                var lookup = GetOrCreateLookup(locator.ViewModelType);
                lookup.Add(locator);
            }

            private ViewModelActionLookup GetOrCreateLookup(Type viewModelType)
            {
                var key = viewModelType.FullName;
                if (key == null)
                    throw new MvxException("Internal logic error - R# warned me?!");

                ViewModelActionLookup toReturn;
                if (!TryGetValue(key, out toReturn))
                {
                    toReturn = new ViewModelActionLookup();
                    this[key] = toReturn;
                }

                return toReturn;
            }
        }

        #endregion
    }
}