// MvxNavigatingObject.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.ViewModels
{
    // This class has a long and twisted history of names - I wonder if it will ever find a really good one?
    public abstract class MvxNavigatingObject
        : MvxNotifyPropertyChanged
    {
        protected IMvxViewDispatcher ViewDispatcher
        {
            get { return (IMvxViewDispatcher) base.Dispatcher; }
        }

        protected bool ShowViewModel<TViewModel>() where TViewModel : IMvxViewModel
        {
            return ShowViewModel<TViewModel>(null, false, MvxRequestedBy.UserAction);
        }

        protected bool ShowViewModel(Type viewModelType)
        {
            return ShowViewModel(viewModelType, null, false, MvxRequestedBy.UserAction);
        }

        protected bool ShowViewModel<TViewModel>(bool clearTop) where TViewModel : IMvxViewModel
        {
            return ShowViewModel<TViewModel>(null, clearTop, MvxRequestedBy.UserAction);
        }

        protected bool ShowViewModel(Type viewModelType, bool clearTop)
        {
            return ShowViewModel(viewModelType, null, clearTop, MvxRequestedBy.UserAction);
        }

        protected bool ShowViewModel<TViewModel>(object parameterValuesObject) where TViewModel : IMvxViewModel
        {
            return ShowViewModel<TViewModel>(parameterValuesObject.ToSimplePropertyDictionary());
        }

        protected bool ShowViewModel(Type viewModelType, object parameterValuesObject)
        {
            return ShowViewModel(viewModelType, parameterValuesObject.ToSimplePropertyDictionary(), false,
                                 MvxRequestedBy.UserAction);
        }

        protected bool ShowViewModel<TViewModel>(object parameterValuesObject, bool clearTop,
                                                 MvxRequestedBy requestedBy) where TViewModel : IMvxViewModel
        {
            return ShowViewModel<TViewModel>(parameterValuesObject.ToSimplePropertyDictionary(), clearTop);
        }

        protected bool ShowViewModel<TViewModel>(IDictionary<string, object> parameterValues)
            where TViewModel : IMvxViewModel
        {
            return ShowViewModel<TViewModel>(parameterValues.ToSimpleStringPropertyDictionary(), false);
        }

        protected bool ShowViewModel<TViewModel>(IDictionary<string, string> parameterValues)
            where TViewModel : IMvxViewModel
        {
            return ShowViewModel<TViewModel>(parameterValues, false);
        }

        protected bool ShowViewModel<TViewModel>(IDictionary<string, string> parameterValues, bool clearTop)
            where TViewModel : IMvxViewModel
        {
            return ShowViewModel<TViewModel>(parameterValues, clearTop, MvxRequestedBy.UserAction);
        }

        protected bool ShowViewModel<TViewModel>(IDictionary<string, string> parameterValues, bool clearTop,
                                                 MvxRequestedBy requestedBy)
            where TViewModel : IMvxViewModel
        {
            return ShowViewModel(typeof (TViewModel), parameterValues, clearTop, requestedBy);
        }

        protected bool ShowViewModel(Type viewModelType, IDictionary<string, string> parameterValues, bool clearTop,
                                     MvxRequestedBy requestedBy)
        {
            MvxTrace.Trace("Showing ViewModel {0}", viewModelType.Name);
            if (Dispatcher != null)
                return ViewDispatcher.ShowViewModel(new MvxViewModelRequest(
                                                        viewModelType,
                                                        parameterValues,
                                                        clearTop,
                                                        requestedBy));

            return false;
        }
    }
}