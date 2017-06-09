// MvxNavigatingObject.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using MvvmCross.Core.Platform;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Core.ViewModels
{
    public abstract class MvxNavigatingObject
        : MvxNotifyPropertyChanged
    {
        protected IMvxViewDispatcher ViewDispatcher => (IMvxViewDispatcher)Dispatcher;

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

        /// <summary>
        /// ShowViewModel with non-primitive type object using json to pass object to the next ViewModel
        /// Be aware that pasing big objects will block your UI, and should be handled async by yourself
        /// </summary>
        /// <param name="parameter">The generic object you want to pass onto the next ViewModel</param>
        protected bool ShowViewModel<TViewModel, TParameter>(TParameter parameter,
                                                 IMvxBundle presentationBundle = null)
            where TViewModel : IMvxViewModel<TParameter> where TParameter : class
        {
            IMvxJsonConverter serializer;
            if (!Mvx.TryResolve(out serializer))
            {
                throw new MvxIoCResolveException("There is no implementation of IMvxJsonConverter registered. You need to use the MvvmCross Json plugin or create your own implementation of IMvxJsonConverter.");
            }

            var json = serializer.SerializeObject(parameter);
            return ShowViewModel<TViewModel>(new Dictionary<string, string> { { "parameter", json } }, presentationBundle);
        }

        protected bool ShowViewModel<TViewModel>(object parameterValuesObject,
                                                 IMvxBundle presentationBundle = null)
            where TViewModel : IMvxViewModel
        {
            return ShowViewModel(
                typeof(TViewModel),
                parameterValuesObject.ToSimplePropertyDictionary(),
                presentationBundle);
        }

        protected bool ShowViewModel<TViewModel>(IDictionary<string, string> parameterValues,
                                                 IMvxBundle presentationBundle = null)
            where TViewModel : IMvxViewModel
        {
            return ShowViewModel(
                typeof(TViewModel),
                new MvxBundle(parameterValues.ToSimplePropertyDictionary()),
                presentationBundle);
        }

        protected bool ShowViewModel<TViewModel>(IMvxBundle parameterBundle = null,
                                                 IMvxBundle presentationBundle = null)
            where TViewModel : IMvxViewModel
        {
            return ShowViewModel(
                typeof(TViewModel),
                parameterBundle,
                presentationBundle);
        }

        protected bool ShowViewModel(Type viewModelType,
                                     object parameterValuesObject,
                                     IMvxBundle presentationBundle = null)
        {
            return ShowViewModel(viewModelType,
                                 new MvxBundle(parameterValuesObject.ToSimplePropertyDictionary()),
                                 presentationBundle);
        }

        protected bool ShowViewModel(Type viewModelType,
                                     IDictionary<string, string> parameterValues,
                                     IMvxBundle presentationBundle = null)
        {
            return ShowViewModel(viewModelType,
                                 new MvxBundle(parameterValues),
                                 presentationBundle);
        }

        protected bool ShowViewModel(Type viewModelType,
                                     IMvxBundle parameterBundle = null,
                                     IMvxBundle presentationBundle = null)
        {
            return ShowViewModelImpl(viewModelType, parameterBundle, presentationBundle);
        }

        private bool ShowViewModelImpl(Type viewModelType, IMvxBundle parameterBundle, IMvxBundle presentationBundle)
        {
            MvxTrace.Trace("Showing ViewModel {0}", viewModelType.Name);
            var viewDispatcher = ViewDispatcher;
            if (viewDispatcher != null)
                return viewDispatcher.ShowViewModel(new MvxViewModelRequest(
                                                        viewModelType,
                                                        parameterBundle,
                                                        presentationBundle));

            return false;
        }
    }
}