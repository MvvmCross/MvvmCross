    // MvxPresentationRequester.cs
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
    public abstract class MvxPresentationRequester
        : MvxNotifyPropertyChanged
    {
        protected IMvxViewDispatcher ViewDispatcher
        {
            get { return (IMvxViewDispatcher) base.Dispatcher; }
        }

        protected bool ChangePresentation(MvxPresentationHint hint)
        {
            MvxTrace.Trace("Requesting presentation change");
            var viewDispatcher = ViewDispatcher;
            if (viewDispatcher != null)
                return viewDispatcher.ChangePresentation(hint);

            return false;
        }

        protected bool ShowViewModel<TViewModel>() where TViewModel : IMvxViewModel
        {
            return ShowViewModel<TViewModel>((IMvxBundle)null, null, MvxRequestedBy.UserAction);
        }

        protected bool ShowViewModel(Type viewModelType)
        {
            return ShowViewModel(viewModelType, (IMvxBundle)null, null, MvxRequestedBy.UserAction);
        }

        protected bool ShowViewModel<TViewModel>(MvxPresentationHint presentationHint) where TViewModel : IMvxViewModel
        {
            return ShowViewModel<TViewModel>((IMvxBundle)null, presentationHint, MvxRequestedBy.UserAction);
        }

        protected bool ShowViewModel(Type viewModelType, MvxPresentationHint presentationHint)
        {
            return ShowViewModel(viewModelType, (IMvxBundle)null, presentationHint, MvxRequestedBy.UserAction);
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


        protected bool ShowViewModel<TViewModel>(IMvxBundle bundle)
            where TViewModel : IMvxViewModel
        {
            return ShowViewModel<TViewModel>(bundle.SafeGetData(), null);
        }

        protected bool ShowViewModel<TViewModel>(IMvxBundle bundle, MvxPresentationHint presentationHint)
            where TViewModel : IMvxViewModel
        {
            return ShowViewModel<TViewModel>(bundle.SafeGetData(), presentationHint, MvxRequestedBy.UserAction);
        }

        protected bool ShowViewModel<TViewModel>(IMvxBundle bundle, MvxPresentationHint presentationHint,
                                                 MvxRequestedBy requestedBy)
            where TViewModel : IMvxViewModel
        {
            return ShowViewModel(typeof (TViewModel), bundle, presentationHint, requestedBy);
        }

        protected bool ShowViewModel(Type viewModelType, IMvxBundle bundle, MvxPresentationHint presentationHint,
                                     MvxRequestedBy requestedBy)
        {
            return ShowViewModel(viewModelType, bundle.SafeGetData(), presentationHint, requestedBy);
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
            return ShowViewModelImpl(viewModelType, parameterValues, presentationHint, requestedBy);
        }

        private bool ShowViewModelImpl(Type viewModelType, IDictionary<string, string> parameterValues, MvxPresentationHint presentationHint,
                                     MvxRequestedBy requestedBy)
        {
            MvxTrace.Trace("Showing ViewModel {0}", viewModelType.Name);
            var viewDispatcher = ViewDispatcher;
            if (viewDispatcher != null)
                return viewDispatcher.ShowViewModel(new MvxViewModelRequest(
                                                        viewModelType,
                                                        parameterValues,
                                                        presentationHint,
                                                        requestedBy));

            return false;
        }
    }
}