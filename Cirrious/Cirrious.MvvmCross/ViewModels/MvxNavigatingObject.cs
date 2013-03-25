// MvxNavigatingObject.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Platform;
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
            return ShowViewModel<TViewModel>(null, null, MvxRequestedBy.UserAction);
        }

        protected bool ShowViewModel(Type viewModelType)
        {
            return ShowViewModel(viewModelType, null, null, MvxRequestedBy.UserAction);
        }

        protected bool ShowViewModel<TViewModel>(MvxPresentationHint presentationHint) where TViewModel : IMvxViewModel
        {
            return ShowViewModel<TViewModel>(null, presentationHint, MvxRequestedBy.UserAction);
        }

        protected bool ShowViewModel(Type viewModelType, MvxPresentationHint presentationHint)
        {
            return ShowViewModel(viewModelType, null, presentationHint, MvxRequestedBy.UserAction);
        }

        protected bool ShowViewModel<TViewModel>(object parameterValuesObject) where TViewModel : IMvxViewModel
        {
            return ShowViewModel<TViewModel>(parameterValuesObject.ToSimplePropertyDictionary());
        }

        protected bool ShowViewModel(Type viewModelType, object parameterValuesObject)
        {
            return ShowViewModel(viewModelType, parameterValuesObject.ToSimplePropertyDictionary(), null,
                                 MvxRequestedBy.UserAction);
        }

        protected bool ShowViewModel<TViewModel>(object parameterValuesObject, MvxPresentationHint presentationHint,
                                                 MvxRequestedBy requestedBy) where TViewModel : IMvxViewModel
        {
            return ShowViewModel<TViewModel>(parameterValuesObject.ToSimplePropertyDictionary(), presentationHint);
        }

        protected bool ShowViewModel<TViewModel>(IDictionary<string, object> parameterValues)
            where TViewModel : IMvxViewModel
        {
            return ShowViewModel<TViewModel>(parameterValues.ToSimpleStringPropertyDictionary(), null);
        }

        protected bool ShowViewModel<TViewModel>(IDictionary<string, string> parameterValues)
            where TViewModel : IMvxViewModel
        {
            return ShowViewModel<TViewModel>(parameterValues, null);
        }

        protected bool ShowViewModel<TViewModel>(IDictionary<string, string> parameterValues, MvxPresentationHint presentationHint)
            where TViewModel : IMvxViewModel
        {
            return ShowViewModel<TViewModel>(parameterValues, presentationHint, MvxRequestedBy.UserAction);
        }

        protected bool ShowViewModel<TViewModel>(IDictionary<string, string> parameterValues, MvxPresentationHint presentationHint,
                                                 MvxRequestedBy requestedBy)
            where TViewModel : IMvxViewModel
        {
            return ShowViewModel(typeof (TViewModel), parameterValues, presentationHint, requestedBy);
        }

        protected bool ShowViewModel(Type viewModelType, IDictionary<string, string> parameterValues, MvxPresentationHint presentationHint,
                                     MvxRequestedBy requestedBy)
        {
            MvxTrace.Trace("Showing ViewModel {0}", viewModelType.Name);
            if (Dispatcher != null)
                return ViewDispatcher.ShowViewModel(new MvxViewModelRequest(
                                                        viewModelType,
                                                        parameterValues,
                                                        presentationHint,
                                                        requestedBy));

            return false;
        }
    }
}