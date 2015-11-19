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
    public abstract class MvxNavigatingObject
        : MvxNotifyPropertyChanged
    {
        protected IMvxViewDispatcher ViewDispatcher => (IMvxViewDispatcher) base.Dispatcher;

        protected bool Close(IMvxViewModel viewModel)
        {
            return ChangePresentation(new MvxClosePresentationHint(viewModel));
        }

        protected bool ChangePresentation(MvxPresentationHint hint)
        {
            MvxTrace.Trace("Requesting presentation change");
            var viewDispatcher = ViewDispatcher;
            if (viewDispatcher != null)
                return viewDispatcher.ChangePresentation(hint);

            return false;
        }

        protected bool ShowViewModel<TViewModel>(object parameterValuesObject,
                                                 IMvxBundle presentationBundle = null,
                                                 MvxRequestedBy requestedBy = null)
            where TViewModel : IMvxViewModel
        {
            return ShowViewModel(
                typeof (TViewModel),
                parameterValuesObject.ToSimplePropertyDictionary(),
                presentationBundle,
                requestedBy);
        }

        protected bool ShowViewModel<TViewModel>(IDictionary<string, string> parameterValues,
                                                 IMvxBundle presentationBundle = null,
                                                 MvxRequestedBy requestedBy = null)
            where TViewModel : IMvxViewModel
        {
            return ShowViewModel(
                typeof (TViewModel),
                new MvxBundle(parameterValues.ToSimplePropertyDictionary()),
                presentationBundle,
                requestedBy);
        }

        protected bool ShowViewModel<TViewModel>(IMvxBundle parameterBundle = null,
                                                 IMvxBundle presentationBundle = null,
                                                 MvxRequestedBy requestedBy = null)
            where TViewModel : IMvxViewModel
        {
            return ShowViewModel(
                typeof (TViewModel),
                parameterBundle,
                presentationBundle,
                requestedBy);
        }


        protected bool ShowViewModel(Type viewModelType,
                                     object parameterValuesObject,
                                     IMvxBundle presentationBundle = null,
                                     MvxRequestedBy requestedBy = null)
        {
            return ShowViewModel(viewModelType,
                                 new MvxBundle(parameterValuesObject.ToSimplePropertyDictionary()),
                                 presentationBundle,
                                 requestedBy);
        }

        protected bool ShowViewModel(Type viewModelType,
                                     IDictionary<string, string> parameterValues,
                                     IMvxBundle presentationBundle = null,
                                     MvxRequestedBy requestedBy = null)
        {
            return ShowViewModel(viewModelType,
                                 new MvxBundle(parameterValues),
                                 presentationBundle,
                                 requestedBy);
        }

        protected bool ShowViewModel(Type viewModelType,
                                     IMvxBundle parameterBundle = null,
                                     IMvxBundle presentationBundle = null,
                                     MvxRequestedBy requestedBy = null)
        {
            return ShowViewModelImpl(viewModelType, parameterBundle, presentationBundle, requestedBy);
        }


        private bool ShowViewModelImpl(Type viewModelType, IMvxBundle parameterBundle, IMvxBundle presentationBundle,
                                       MvxRequestedBy requestedBy)
        {
            MvxTrace.Trace("Showing ViewModel {0}", viewModelType.Name);
            var viewDispatcher = ViewDispatcher;
            if (viewDispatcher != null)
                return viewDispatcher.ShowViewModel(new MvxViewModelRequest(
                                                        viewModelType,
                                                        parameterBundle,
                                                        presentationBundle,
                                                        requestedBy));

            return false;
        }
    }
}