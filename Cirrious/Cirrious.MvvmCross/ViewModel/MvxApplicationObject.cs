#region Copyright

// <copyright file="MvxApplicationObject.cs" company="Cirrious">
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
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Localization;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModel;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.ViewModel
{
    public class MvxApplicationObject 
        : MvxNotifyProperty
        , IMvxServiceConsumer<IMvxTextProvider>
    {
        private readonly string _cachedNamespace;
        private readonly string _cachedTypeName;

        private IMvxTextProvider _cachedTextProvider;
        private IMvxTextProvider TextProvider
        {
            get
            {
                if (_cachedTextProvider != null)
                    return _cachedTextProvider;

                lock (this)
                {
                    _cachedTextProvider = this.GetService<IMvxTextProvider>();
                    if (_cachedTextProvider == null)
                    {
                        throw new MvxException("Missing text provider - please initialise IoC with a suitable IMvxTextProvider");
                    }
                    return _cachedTextProvider;
                }
            }
        }

        protected MvxApplicationObject()
        {
            _cachedNamespace = GetType().Namespace;
            _cachedTypeName = GetType().Name;
        }

        protected string GetText(string entryKey)
        {
            return GetText(_cachedNamespace, _cachedTypeName, entryKey);
        }

        protected virtual string GetText(string typeName, string entryKey)
        {
            return GetText(_cachedNamespace, typeName, entryKey);
        }

        protected virtual string GetText(string namespaceKey, string typeKey, string entryKey)
        {
            return TextProvider.GetText(namespaceKey, typeKey, entryKey);
        }

        protected virtual bool RequestMainThreadAction(Action action)
        {
            if (ViewDispatcher != null)
                return ViewDispatcher.RequestMainThreadAction(action);

            return false;
        }

        protected bool RequestNavigate<TViewModel>() where TViewModel : IMvxViewModel
        {
            return RequestNavigate<TViewModel>(null);
        }

        protected bool RequestNavigate<TViewModel>(object parameterValuesObject) where TViewModel : IMvxViewModel
        {
            return RequestNavigate<TViewModel>(parameterValuesObject.ToSimplePropertyDictionary());
        }

        protected bool RequestNavigate<TViewModel>(IDictionary<string, string> parameterValues)
            where TViewModel : IMvxViewModel
        {
            if (ViewDispatcher != null)
                return ViewDispatcher.RequestNavigate(new MvxShowViewModelRequest(
                                                          typeof (TViewModel),
                                                          parameterValues));

            return false;
        }

        protected bool RequestNavigateBack()
        {
            if (ViewDispatcher != null)
                return ViewDispatcher.RequestNavigateBack();

            return false;
        }
    }
}