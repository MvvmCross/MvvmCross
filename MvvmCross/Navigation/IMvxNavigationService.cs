// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
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
        /// Dispatches a ChangePresentation with Hint
        /// </summary>
        /// <param name="hint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> ChangePresentation(MvxPresentationHint hint, CancellationToken cancellationToken = default);

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
        /// Navigates to a ViewModel Type
        /// </summary>
        /// <param name="viewModelType"></param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
        /// <returns>Boolean indicating successful navigation</returns>
        Task<bool> Navigate([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Navigates to a ViewModel Type and passes TParameter
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="viewModelType"></param>
        /// <param name="param"></param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
        /// <returns>Boolean indicating successful navigation</returns>
        Task<bool> Navigate<TParameter>([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType, TParameter param, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Translates the provided Uri to a ViewModel request and dispatches it.
        /// </summary>
        /// <param name="path">URI to route</param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
        /// <returns>Boolean indicating successful navigation</returns>
        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
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
        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
        Task<bool> Navigate<TParameter>(string path, TParameter param,
            IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Navigate to a ViewModel determined by its type
        /// </summary>
        /// <param name="presentationBundle">(optional) presentation bundle</param>
        /// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
        /// <typeparam name="TViewModel">Type of <see cref="IMvxViewModel"/></typeparam>
        /// <returns>Boolean indicating successful navigation</returns>
        Task<bool> Navigate<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>(
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
        Task<bool> Navigate<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel, TParameter>(
            TParameter param, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
                where TViewModel : IMvxViewModel<TParameter>;

        /// <summary>
        ///     Loads a view model targeting the window for the given source.
        /// </summary>
        /// <typeparam name="TViewModel">The viewmodel type.</typeparam>
        /// <typeparam name="TParameter">The parameter type.</typeparam>
        /// <param name="param">The parameter value.</param>
        /// <param name="source">
        ///     This is used to find the window to execute the navigate in.
        ///     This is usually the viewmodel instance which calls this method. 
        /// </param>
        /// <param name="presentationBundle">The presentation bungle.</param>
        /// <param name="cancellationToken">Any cancellation token.</param>
        /// <returns>True if navigation was successful.</returns>
        Task<bool> Navigate<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel, TParameter>(TParameter param, IMvxViewModel source, IMvxBundle? presentationBundle = null,
            CancellationToken cancellationToken = default) where TViewModel : IMvxViewModel<TParameter>
            where TParameter : notnull;

        /// <summary>
        ///     Loads a view model targeting the window for the given source.
        /// </summary>
        /// <typeparam name="TParameter">The parameter</typeparam>
        /// <param name="viewModelType">The viewmodel type.</param>
        /// <param name="param">The parameter value.</param>
        /// <param name="source">
        ///     This is used to find the window to execute the navigate in.
        ///     This is usually the viewmodel instance which calls this method. 
        /// </param>
        /// <param name="presentationBundle">The presentation bungle.</param>
        /// <param name="cancellationToken">Any cancellation token.</param>
        /// <returns>True if navigation was successful.</returns>
        Task<bool> Navigate<TParameter>([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType, TParameter param, IMvxViewModel source, IMvxBundle? presentationBundle = null,
            CancellationToken cancellationToken = default)
            where TParameter : notnull;

        /// <summary>
        ///     Navigates to a view for the given type.
        /// </summary>
        /// <param name="viewModelType">The type of the viewmodel to navigate to.</param>
        /// <param name="source">
        ///     This is used to find the window to execute the navigate in.
        ///     This is usually the viewmodel instance which calls this method. 
        /// </param>
        /// <param name="presentationBundle">A presentation bundle.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<bool> Navigate([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType, IMvxViewModel source, IMvxBundle? presentationBundle = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Navigates to the viewmodel for the given type.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the viewmodel to navigate to.</typeparam>
        /// <param name="source">
        ///     This is used to find the window to execute the navigate in.
        ///     This is usually the viewmodel instance which calls this method. 
        /// </param>
        /// <param name="presentationBundle">The presentation bundle.</param>
        /// <param name="cancellationToken">Any cancellation token.</param>
        /// <returns>True if successful, false otherwise.</returns>
        Task<bool> Navigate<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>(IMvxViewModel source,
            IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
            where TViewModel : IMvxViewModel;

        /// <summary>
        ///     Navigates to a view for the given viewmodel.
        /// </summary>
        /// <param name="viewModel">The viewmodel to navigate to.</param>
        /// <param name="source">
        ///     This is used to find the window to execute the navigate in.
        ///     This is usually the viewmodel instance which calls this method. 
        /// </param>
        /// <param name="presentationBundle">The presentation bundle.</param>
        /// <param name="cancellationToken">Any cancellation token.</param>
        /// <returns>True if successful, false otherwise.</returns>
        Task<bool> Navigate(
            IMvxViewModel viewModel, IMvxViewModel source, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Navigates to a view for the given viewmodel.
        /// </summary>
        /// <typeparam name="TParameter">The parameter type.</typeparam>
        /// <param name="viewModel">The viewmodel to navigate to.</param>
        /// <param name="param">Any parameters.</param>
        /// <param name="source">
        ///     This is used to find the window to execute the navigate in.
        ///     This is usually the viewmodel instance which calls this method. 
        /// </param>
        /// <param name="presentationBundle">The presentation bundle.</param>
        /// <param name="cancellationToken">Any cancellation token.</param>
        /// <returns>True if successful, false otherwise.</returns>
        Task<bool> Navigate<TParameter>(IMvxViewModel<TParameter> viewModel, TParameter param, IMvxViewModel source,
            IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
            where TParameter : notnull;
    }
#nullable restore
}
