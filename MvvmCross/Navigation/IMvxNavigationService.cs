// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Navigation.EventArguments;
using MvvmCross.ViewModels;

namespace MvvmCross.Navigation
{
#nullable enable
    /// <summary>
    /// Allows for Task and URI based navigation in MvvmCross
    /// </summary>
    public interface IMvxNavigationService
    {
        /// <summary>
        /// Event that triggers right before navigation happens
        /// </summary>
        event EventHandler<IMvxNavigateEventArgs>? WillNavigate;

        /// <summary>
        /// Event that triggers right after navigation did occur
        /// </summary>
        event EventHandler<IMvxNavigateEventArgs>? DidNavigate;

        /// <summary>
        /// Event that triggers right before Closing
        /// </summary>
        event EventHandler<IMvxNavigateEventArgs>? WillClose;

        /// <summary>
        /// Event that triggers right after did happen
        /// </summary>
        event EventHandler<IMvxNavigateEventArgs>? DidClose;

        /// <summary>
        /// Event that triggers when presentation will change
        /// </summary>
        event EventHandler<ChangePresentationEventArgs>? WillChangePresentation;

        /// <summary>
        /// Event that triggers when presentation changed
        /// </summary>
        event EventHandler<ChangePresentationEventArgs>? DidChangePresentation;

        /// <summary>
        /// Loads all navigation routes based on the referenced assemblies
        /// </summary>
        /// <param name="assemblies">The assemblies that should be indexed for routes</param>
        void LoadRoutes(IEnumerable<Assembly> assemblies);

        /// <summary>
        /// Navigates to an instance of a ViewModel
        /// </summary>
        /// <param name="viewModel">ViewModel to navigate to</param>
        /// <param name="presentationBundle">(optional) presentation bundle</param>
        /// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
        /// <returns>Boolean indicating successful navigation</returns>
        Task<bool> Navigate(IMvxViewModel viewModel, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Navigates to an instance of a ViewModel and passes TParameter
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="viewModel">ViewModel to navigate to</param>
        /// <param name="param">ViewModel parameter</param>
        /// <param name="presentationBundle">(optional) presentation bundle</param>
        /// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
        /// <returns>Boolean indicating successful navigation</returns>
        Task<bool> Navigate<TParameter>(IMvxViewModel<TParameter> viewModel, TParameter param,
            IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Navigates to an instance of a ViewModel and returns TResult
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
        /// <returns></returns>
        Task<TResult> Navigate<TResult>(IMvxViewModelResult<TResult> viewModel, IMvxBundle? presentationBundle = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Navigates to an instance of a ViewModel passes TParameter and returns TResult
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="param"></param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
        /// <returns></returns>
        Task<TResult> Navigate<TParameter, TResult>(IMvxViewModel<TParameter, TResult> viewModel, TParameter param,
            IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Navigates to a ViewModel Type
        /// </summary>
        /// <param name="viewModelType"></param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
        /// <returns>Boolean indicating successful navigation</returns>
        Task<bool> Navigate(Type viewModelType, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Navigates to a ViewModel Type and passes TParameter
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="viewModelType"></param>
        /// <param name="param"></param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
        /// <returns>Boolean indicating successful navigation</returns>
        Task<bool> Navigate<TParameter>(Type viewModelType, TParameter param, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Navigates to a ViewModel Type passes and returns TResult
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="viewModelType"></param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
        /// <returns></returns>
        Task<TResult> Navigate<TResult>(Type viewModelType, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Navigates to a ViewModel Type passes TParameter and returns TResult
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="viewModelType"></param>
        /// <param name="param"></param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
        /// <returns></returns>
        Task<TResult> Navigate<TParameter, TResult>(Type viewModelType, TParameter param,
            IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Translates the provided Uri to a ViewModel request and dispatches it.
        /// </summary>
        /// <param name="path">URI to route</param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
        /// <returns>Boolean indicating successful navigation</returns>
        Task<bool> Navigate(
            string path, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Translates the provided Uri to a ViewModel request and dispatches it.
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="path"></param>
        /// <param name="param"></param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
        /// <returns>Boolean indicating successful navigation</returns>
        Task<bool> Navigate<TParameter>(string path, TParameter param,
            IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Translates the provided Uri to a ViewModel request and dispatches it.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="path"></param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
        /// <returns></returns>
        Task<TResult> Navigate<TResult>(string path, IMvxBundle? presentationBundle = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Translates the provided Uri to a ViewModel request and dispatches it.
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="path"></param>
        /// <param name="param"></param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
        /// <returns></returns>
        Task<TResult> Navigate<TParameter, TResult>(string path, TParameter param,
            IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Navigate to a ViewModel determined by its type
        /// </summary>
        /// <param name="presentationBundle">(optional) presentation bundle</param>
        /// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
        /// <typeparam name="TViewModel">Type of <see cref="IMvxViewModel"/></typeparam>
        /// <returns>Boolean indicating successful navigation</returns>
        Task<bool> Navigate<TViewModel>(
            IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
            where TViewModel : IMvxViewModel;

        /// <summary>
        /// Navigate to a ViewModel determined by its type, with parameter
        /// </summary>
        /// <param name="param">ViewModel parameter</param>
        /// <param name="presentationBundle">(optional) presentation bundle</param>
        /// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
        /// <typeparam name="TViewModel">Type of <see cref="IMvxViewModel{Parameter}"/></typeparam>
        /// <typeparam name="TParameter">Parameter passed to ViewModel</typeparam>
        /// <returns>Boolean indicating successful navigation</returns>
        Task<bool> Navigate<TViewModel, TParameter>(
            TParameter param, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
            where TViewModel : IMvxViewModel<TParameter>;

        /// <summary>
        /// Navigate to a ViewModel determined by its type, which returns a result.
        /// </summary>
        /// <param name="presentationBundle">(optional) presentation bundle</param>
        /// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
        /// <typeparam name="TViewModel">Type of <see cref="IMvxViewModel"/></typeparam>
        /// <typeparam name="TResult">Result from the ViewModel</typeparam>
        /// <returns>Returns a <see cref="Task{Task}"/> with <see cref="TResult"/></returns>
        Task<TResult> Navigate<TViewModel, TResult>(
            IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
            where TViewModel : IMvxViewModelResult<TResult>;

        /// <summary>
        /// Navigate to a ViewModel determined by its type, with parameter and which returns a result.
        /// </summary>
        /// <param name="param">ViewModel parameter</param>
        /// <param name="presentationBundle">(optional) presentation bundle</param>
        /// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
        /// <typeparam name="TViewModel">Type of <see cref="IMvxViewModel{Parameter,Result}"/></typeparam>
        /// <typeparam name="TParameter">Parameter passed to ViewModel</typeparam>
        /// <typeparam name="TResult">Result from the ViewModel</typeparam>
        /// <returns>Returns a <see cref="Task{Task}"/> with <see cref="TResult"/></returns>
        Task<TResult> Navigate<TViewModel, TParameter, TResult>(
            TParameter param, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
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
        Task<bool> CanNavigate<TViewModel>()
            where TViewModel : IMvxViewModel;

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
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> Close(IMvxViewModel viewModel, CancellationToken cancellationToken = default);

        /// <summary>
        /// Closes the View attached to the ViewModel and returns a result to the underlying ViewModel
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="result"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> Close<TResult>(IMvxViewModelResult<TResult> viewModel, TResult result, CancellationToken cancellationToken = default);

        /// <summary>
        /// Dispatches a ChangePresentation with Hint
        /// </summary>
        /// <param name="hint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> ChangePresentation(MvxPresentationHint hint, CancellationToken cancellationToken = default);
    }
#nullable restore
}
