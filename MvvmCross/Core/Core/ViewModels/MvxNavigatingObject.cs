// MvxNavigatingObject.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.ViewModels
{
    using System;
    using System.Collections.Generic;

    using MvvmCross.Core.Platform;
    using MvvmCross.Core.Views;
    using MvvmCross.Platform.Platform;
    using MvvmCross.Platform;
    public abstract class MvxNavigatingObject
        : MvxNotifyPropertyChanged
    {
        protected IMvxViewDispatcher ViewDispatcher => (IMvxViewDispatcher)base.Dispatcher;

        protected bool Close(IMvxViewModel viewModel)
        {
            return this.ChangePresentation(new MvxClosePresentationHint(viewModel));
        }

        protected bool ChangePresentation(MvxPresentationHint hint)
        {
            MvxTrace.Trace("Requesting presentation change");
            var viewDispatcher = this.ViewDispatcher;
            if (viewDispatcher != null)
                return viewDispatcher.ChangePresentation(hint);

            return false;
        }

		/// <summary>
		///     ShowViewModel with non-primitive type object using json to pass object to the next ViewModel
		/// 	Be aware that pasing big objects will block your UI, and should be handled async by yourself
		/// </summary>
		/// <param name="parameter">The generic object you want to pass onto the next ViewModel</param>
        protected bool ShowViewModel<TViewModel, TInit>(TInit parameter,
												 IMvxBundle presentationBundle = null,
												 MvxRequestedBy requestedBy = null) where TViewModel : MvxViewModel<TInit>
        {
            IMvxJsonConverter serializer;
            if (!Mvx.TryResolve(out serializer))
            {
                Mvx.Trace(
                    "Could not resolve IMvxJsonConverter, it is going to be hard to initialize with custom object");
                return false;
            }

            var json = serializer.SerializeObject(parameter);
            return this.ShowViewModel<TViewModel>(new Dictionary<string, string> { { "parameter", json } }, presentationBundle, requestedBy);
        }

        protected bool ShowViewModel<TViewModel>(object parameterValuesObject,
                                                 IMvxBundle presentationBundle = null,
                                                 MvxRequestedBy requestedBy = null)
            where TViewModel : IMvxViewModel
        {
            return this.ShowViewModel(
                typeof(TViewModel),
                parameterValuesObject.ToSimplePropertyDictionary(),
                presentationBundle,
                requestedBy);
        }

        protected bool ShowViewModel<TViewModel>(IDictionary<string, string> parameterValues,
                                                 IMvxBundle presentationBundle = null,
                                                 MvxRequestedBy requestedBy = null)
            where TViewModel : IMvxViewModel
        {
            return this.ShowViewModel(
                typeof(TViewModel),
                new MvxBundle(parameterValues.ToSimplePropertyDictionary()),
                presentationBundle,
                requestedBy);
        }

        protected bool ShowViewModel<TViewModel>(IMvxBundle parameterBundle = null,
                                                 IMvxBundle presentationBundle = null,
                                                 MvxRequestedBy requestedBy = null)
            where TViewModel : IMvxViewModel
        {
            return this.ShowViewModel(
                typeof(TViewModel),
                parameterBundle,
                presentationBundle,
                requestedBy);
        }

        protected bool ShowViewModel(Type viewModelType,
                                     object parameterValuesObject,
                                     IMvxBundle presentationBundle = null,
                                     MvxRequestedBy requestedBy = null)
        {
            return this.ShowViewModel(viewModelType,
                                 new MvxBundle(parameterValuesObject.ToSimplePropertyDictionary()),
                                 presentationBundle,
                                 requestedBy);
        }

        protected bool ShowViewModel(Type viewModelType,
                                     IDictionary<string, string> parameterValues,
                                     IMvxBundle presentationBundle = null,
                                     MvxRequestedBy requestedBy = null)
        {
            return this.ShowViewModel(viewModelType,
                                 new MvxBundle(parameterValues),
                                 presentationBundle,
                                 requestedBy);
        }

        protected bool ShowViewModel(Type viewModelType,
                                     IMvxBundle parameterBundle = null,
                                     IMvxBundle presentationBundle = null,
                                     MvxRequestedBy requestedBy = null)
        {
            return this.ShowViewModelImpl(viewModelType, parameterBundle, presentationBundle, requestedBy);
        }

        private bool ShowViewModelImpl(Type viewModelType, IMvxBundle parameterBundle, IMvxBundle presentationBundle,
                                       MvxRequestedBy requestedBy)
        {
            MvxTrace.Trace("Showing ViewModel {0}", viewModelType.Name);
            var viewDispatcher = this.ViewDispatcher;
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