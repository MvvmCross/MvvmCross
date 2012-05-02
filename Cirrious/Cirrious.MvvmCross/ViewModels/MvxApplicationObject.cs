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
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Platform.Diagnostics;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.ViewModels
{
    public class MvxApplicationObject 
        : MvxNotifyPropertyChanged
    {
        protected MvxApplicationObject()
        {
        }

        #region Main thread actions and navigation requests

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