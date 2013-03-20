// MvxNavigatingObject.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.ViewModels
{
    public abstract class MvxNavigatingObject
        : MvxNotifyPropertyChanged
    {
        protected IMvxViewDispatcher ViewDispatcher
        {
            get
            {
                return (IMvxViewDispatcher)base.Dispatcher;
            }
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
            return RequestNavigate(viewModelType, parameterValuesObject.ToSimplePropertyDictionary(), false,
                                   MvxRequestedBy.UserAction);
        }

        protected bool RequestNavigate<TViewModel>(object parameterValuesObject, bool clearTop,
                                                   MvxRequestedBy requestedBy) where TViewModel : IMvxViewModel
        {
            return RequestNavigate<TViewModel>(parameterValuesObject.ToSimplePropertyDictionary(), clearTop);
        }

        protected bool RequestNavigate<TViewModel>(IDictionary<string, object> parameterValues)
            where TViewModel : IMvxViewModel
        {
            return RequestNavigate<TViewModel>(parameterValues.ToSimpleStringPropertyDictionary(), false);
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

        protected bool RequestNavigate<TViewModel>(IDictionary<string, string> parameterValues, bool clearTop,
                                                   MvxRequestedBy requestedBy)
            where TViewModel : IMvxViewModel
        {
            return RequestNavigate(typeof (TViewModel), parameterValues, clearTop, requestedBy);
        }

        protected bool RequestNavigate(Type viewModelType, IDictionary<string, string> parameterValues, bool clearTop,
                                       MvxRequestedBy requestedBy)
        {
            MvxTrace.TaggedTrace("Navigation", "Navigate to " + viewModelType.Name + " with args");
            if (Dispatcher != null)
                return ViewDispatcher.RequestNavigate(new MvxShowViewModelRequest(
                                                          viewModelType,
                                                          parameterValues,
                                                          clearTop,
                                                          requestedBy));

            return false;
        }

        #endregion
    }
}