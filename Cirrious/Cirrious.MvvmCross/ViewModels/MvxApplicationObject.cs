#region Copyright
// <copyright file="MvxApplicationObject.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Localization;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Platform.Diagnostics;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.ViewModels
{
    public class MvxApplicationObject 
        : MvxNotifyPropertyChanged
        , IMvxServiceConsumer<IMvxTextProvider>
    {
        #region Nested class used for Language Binding

        private class MvxLanguageBinder : IMvxLanguageBinder
        {
            private readonly MvxApplicationObject _parent;
            private readonly string _namespaceName;
            private readonly string _typeName;

#warning for now not using default parameters anywhere because of some suspected monodroid compiler/runtime issues - check if these are fixed
            public MvxLanguageBinder(MvxApplicationObject parent)
                : this(parent, null, null)
            {
            }

            public MvxLanguageBinder(MvxApplicationObject parent, string namespaceName)
                : this(parent, namespaceName, null)
            {                
            }

            public MvxLanguageBinder(MvxApplicationObject parent, string namespaceName, string typeName)
            {
                _parent = parent;
                _namespaceName = namespaceName;
                _typeName = typeName;
            }

            public string GetText(string entryKey)
            {
                return _parent.GetText(_namespaceName, _typeName, entryKey);
            }

            public string GetText(string entryKey, params object[] args)
            {
                return string.Format(GetText(entryKey), args);
            }
        }

        #endregion

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

        #region Language methods

        protected IMvxLanguageBinder CreateLanguageBinder()
        {
            return CreateLanguageBinder(_cachedNamespace, _cachedTypeName);
        }

        protected IMvxLanguageBinder CreateLanguageBinder(string typeName)
        {
            return CreateLanguageBinder(_cachedNamespace, typeName);
        }

        protected IMvxLanguageBinder CreateLanguageBinder(string namespaceName, string typeName)
        {
            return new MvxLanguageBinder(this, namespaceName, typeName);
        }

        private string GetText(string namespaceKey, string typeKey, string entryKey)
        {
            return TextProvider.GetText(namespaceKey, typeKey, entryKey);
        }

        #endregion

        #region Main thread actions and navigation requests

#warning How does this now clash with InvokeOnMainThread?
        protected virtual bool RequestMainThreadAction(Action action)
        {
            if (ViewDispatcher != null)
                return ViewDispatcher.RequestMainThreadAction(action);

            return false;
        }

        protected bool RequestNavigate<TViewModel>() where TViewModel : IMvxViewModel
        {
			return RequestNavigate<TViewModel>(null, false, MvxRequestedBy.UserAction);
        }

        protected bool RequestNavigate(Type viewModelType)
        {
            return RequestNavigate(viewModelType, null, false, MvxRequestedBy.UserAction);
        }

        protected bool RequestNavigate<TViewModel>(bool clearTop) where TViewModel : IMvxViewModel
        {
            return RequestNavigate<TViewModel>(null, clearTop, MvxRequestedBy.UserAction);
        }

        protected bool RequestNavigate(Type viewModelType, bool clearTop)
        {
            return RequestNavigate(viewModelType, null, clearTop, MvxRequestedBy.UserAction);
        }

        protected bool RequestNavigate<TViewModel>(object parameterValuesObject) where TViewModel : IMvxViewModel
        {
            return RequestNavigate<TViewModel>(parameterValuesObject.ToSimplePropertyDictionary());
        }

        protected bool RequestNavigate(Type viewModelType, object parameterValuesObject)
        {
            return RequestNavigate(viewModelType, parameterValuesObject.ToSimplePropertyDictionary(), false, MvxRequestedBy.UserAction);
        }

        protected bool RequestNavigate<TViewModel>(object parameterValuesObject, bool clearTop, MvxRequestedBy requestedBy) where TViewModel : IMvxViewModel
        {
            return RequestNavigate<TViewModel>(parameterValuesObject.ToSimplePropertyDictionary(), clearTop);
        }

        protected bool RequestNavigate<TViewModel>(IDictionary<string, string> parameterValues)
            where TViewModel : IMvxViewModel
        {
            return RequestNavigate<TViewModel>(parameterValues, false);
        }

        protected bool RequestNavigate<TViewModel>(IDictionary<string, string> parameterValues, bool clearTop)
            where TViewModel : IMvxViewModel
        {
            return RequestNavigate<TViewModel>(parameterValues, clearTop, MvxRequestedBy.UserAction);
        }

        protected bool RequestNavigate<TViewModel>(IDictionary<string, string> parameterValues, bool clearTop, MvxRequestedBy requestedBy)
            where TViewModel : IMvxViewModel
        {
            return RequestNavigate(typeof(TViewModel), parameterValues, clearTop, requestedBy);
        }

        protected bool RequestNavigate(Type viewModelType, IDictionary<string, string> parameterValues, bool clearTop, MvxRequestedBy requestedBy)
        {
            MvxTrace.TaggedTrace("Navigation", "Navigate to " + viewModelType.Name + " with args");
            if (ViewDispatcher != null)
                return ViewDispatcher.RequestNavigate(new MvxShowViewModelRequest(
                                                          viewModelType,
                                                          parameterValues,
                                                          clearTop,
                                                          requestedBy));

            return false;
        }

        protected bool RequestClose(IMvxViewModel toClose)
        {
            MvxTrace.TaggedTrace("Navigation", "Close requested");
			if (ViewDispatcher != null)
                return ViewDispatcher.RequestClose(toClose);

            return false;
        }

        #endregion
    }
}