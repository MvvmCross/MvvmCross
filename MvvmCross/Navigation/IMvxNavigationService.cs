// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Navigation.EventArguments;
using MvvmCross.ViewModels;

namespace MvvmCross.Navigation
{
    public delegate void BeforeNavigateEventHandler(object sender, IMvxNavigateEventArgs e);
    public delegate void AfterNavigateEventHandler(object sender, IMvxNavigateEventArgs e);
    public delegate void BeforeCloseEventHandler(object sender, IMvxNavigateEventArgs e);
    public delegate void AfterCloseEventHandler(object sender, IMvxNavigateEventArgs e);
    public delegate void BeforeChangePresentationEventHandler(object sender, ChangePresentationEventArgs e);
    public delegate void AfterChangePresentationEventHandler(object sender, ChangePresentationEventArgs e);

    /// <summary>
    /// Allows for Task and URI based navigation in MvvmCross
    /// </summary>
    public interface IMvxNavigationService
    {
        event BeforeNavigateEventHandler BeforeNavigate;
        event AfterNavigateEventHandler AfterNavigate;
        event BeforeCloseEventHandler BeforeClose;
        event AfterCloseEventHandler AfterClose;
        event BeforeChangePresentationEventHandler BeforeChangePresentation;
        event AfterChangePresentationEventHandler AfterChangePresentation;

        /// <summary>
        /// Navigates to an instance of a ViewModel
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="presentationBundle"></param>
        /// <returns>Boolean indicating successful navigation</returns>
        Task<bool> Navigate(IMvxViewModel viewModel, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Navigates to an instance of a ViewModel and passes TParameter
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="param"></param>
        /// <param name="presentationBundle"></param>
        /// <returns>Boolean indicating successful navigation</returns>
        Task<bool> Navigate<TParameter>(IMvxViewModel<TParameter> viewModel, TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Navigates to an instance of a ViewModel and returns TResult
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TResult> Navigate<TResult>(IMvxViewModelResult<TResult> viewModel, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Navigates to an instance of a ViewModel passes TParameter and returns TResult
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="param"></param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TResult> Navigate<TParameter, TResult>(IMvxViewModel<TParameter, TResult> viewModel, TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Navigates to a ViewModel Type
        /// </summary>
        /// <param name="viewModelType"></param>
        /// <param name="presentationBundle"></param>
        /// <returns>Boolean indicating successful navigation</returns>
        Task<bool> Navigate(Type viewModelType, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Navigates to a ViewModel Type and passes TParameter
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="viewModelType"></param>
        /// <param name="param"></param>
        /// <param name="presentationBundle"></param>
        /// <returns>Boolean indicating successful navigation</returns>
        Task<bool> Navigate<TParameter>(Type viewModelType, TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Navigates to a ViewModel Type passes and returns TResult
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="viewModelType"></param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TResult> Navigate<TResult>(Type viewModelType, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Navigates to a ViewModel Type passes TParameter and returns TResult
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="viewModelType"></param>
        /// <param name="param"></param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TResult> Navigate<TParameter, TResult>(Type viewModelType, TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Translates the provided Uri to a ViewModel request and dispatches it.
        /// </summary>
        /// <param name="path">URI to route</param>
        /// <param name="presentationBundle"></param>
        /// <returns>Boolean indicating successful navigation</returns>
        Task<bool> Navigate(string path, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Translates the provided Uri to a ViewModel request and dispatches it.
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="path"></param>
        /// <param name="param"></param>
        /// <param name="presentationBundle"></param>
        /// <returns>Boolean indicating successful navigation</returns>
        Task<bool> Navigate<TParameter>(string path, TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Translates the provided Uri to a ViewModel request and dispatches it.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="path"></param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TResult> Navigate<TResult>(string path, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Translates the provided Uri to a ViewModel request and dispatches it.
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="path"></param>
        /// <param name="param"></param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TResult> Navigate<TParameter, TResult>(string path, TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> Navigate<TViewModel>(IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
            where TViewModel : IMvxViewModel;

        Task<bool> Navigate<TViewModel, TParameter>(TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
            where TViewModel : IMvxViewModel<TParameter>;

        Task<TResult> Navigate<TViewModel, TResult>(IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
            where TViewModel : IMvxViewModelResult<TResult>;

        Task<TResult> Navigate<TViewModel, TParameter, TResult>(TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
            where TViewModel : IMvxViewModel<TParameter, TResult>;

        /// <summary>
        /// Verifies if the provided Uri can be routed to a ViewModel request.
        /// </summary>
        /// <param name="path">URI to route</param>
        /// <returns>True if the uri can be routed or false if it cannot.</returns>
        Task<bool> CanNavigate(string path);

        /// <summary>
        /// Verifies if the provided viewmodel is available
        /// </summary>
        /// <returns>True if the ViewModel is available</returns>
        Task<bool> CanNavigate<TViewModel>() where TViewModel : IMvxViewModel;

        /// <summary>
        /// Verifies if the provided viewmodel is available
        /// </summary>
        /// <param name="viewModelType">ViewModel type to check</param>
        /// <returns>True if the ViewModel is available</returns>
        Task<bool> CanNavigate(Type viewModelType);

        /// <summary>
        /// Closes the View attached to the ViewModel
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        Task<bool> Close(IMvxViewModel viewModel, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Closes the View attached to the ViewModel and returns a result to the underlaying ViewModel
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        Task<bool> Close<TResult>(IMvxViewModelResult<TResult> viewModel, TResult result, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Dispatches a ChangePresentation with Hint
        /// </summary>
        /// <param name="hint"></param>
        /// <returns></returns>
        Task<bool> ChangePresentation(MvxPresentationHint hint, CancellationToken cancellationToken = default(CancellationToken));
    }
}
