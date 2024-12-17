// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using MvvmCross.ViewModels;
using MvvmCross.ViewModels.Result;

namespace MvvmCross.Navigation;

public static class MvxNavigationExtensions
{
    /// <summary>
    /// Verifies if the provided Uri can be routed to a ViewModel request.
    /// </summary>
    /// <param name="navigationService"></param>
    /// <param name="path">URI to route</param>
    /// <returns>True if the uri can be routed or false if it cannot.</returns>
    public static Task<bool> CanNavigate(this IMvxNavigationService navigationService, Uri path)
    {
        return navigationService.CanNavigate(path.ToString());
    }

    /// <summary>
    /// Translates the provided Uri to a ViewModel request and dispatches it.
    /// </summary>
    /// <param name="navigationService"></param>
    /// <param name="path">URI to route</param>
    /// <param name="presentationBundle"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A task to await upon</returns>
    public static Task Navigate(this IMvxNavigationService navigationService, Uri path, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
    {
        return navigationService.Navigate(path.ToString(), presentationBundle, cancellationToken);
    }

    public static Task Navigate<TParameter>(this IMvxNavigationService navigationService, Uri path, TParameter param, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
    {
        return navigationService.Navigate(path.ToString(), param, presentationBundle, cancellationToken);
    }

    /// <summary>
    /// Navigate from a Result Awaiting ViewModel to a Result Setting ViewModel determined by its type
    /// </summary>
    /// <param name="navigationService">Navigation service</param>
    /// <param name="fromViewModel">Result Awaiting ViewModel</param>
    /// <param name="resultViewModelManager">Result ViewModel Manager</param>
    /// <param name="presentationBundle">(optional) presentation bundle</param>
    /// <param name="cancellationToken">(optional) CancellationToken to cancel the navigation</param>
    /// <typeparam name="TViewModel">Type of <see cref="IMvxResultSettingViewModel{TResult}"/></typeparam>
    /// <typeparam name="TResult">Result awaited by Result Awaiting ViewModel and set by Result Setting ViewModel</typeparam>
    /// <returns>Boolean indicating successful navigation</returns>
    public static async Task<bool> NavigateRegisteringToResult<TViewModel, TResult>(
        this IMvxNavigationService navigationService,
        IMvxResultAwaitingViewModel<TResult> fromViewModel,
        IMvxResultViewModelManager resultViewModelManager,
        IMvxBundle? presentationBundle = null,
        CancellationToken cancellationToken = default)
        where TViewModel : IMvxResultSettingViewModel<TResult>, IMvxViewModel
    {
        bool navigated = await navigationService.Navigate<TViewModel>(presentationBundle, cancellationToken);
        if (navigated)
            fromViewModel.RegisterToResult(resultViewModelManager);
        return navigated;
    }

    /// <summary>
    /// Navigate from a Result Awaiting ViewModel to a Result Setting ViewModel determined by its type, with parameter
    /// </summary>
    /// <param name="navigationService">Navigation service</param>
    /// <param name="fromViewModel">Result Awaiting ViewModel</param>
    /// <param name="resultViewModelManager">Result ViewModel Manager</param>
    /// <param name="parameter">ViewModel parameter</param>
    /// <param name="presentationBundle">(optional) presentation bundle</param>
    /// <param name="cancellationToken">(optional) CancellationToken to cancel the navigation</param>
    /// <typeparam name="TViewModel">Type of <see cref="IMvxResultSettingViewModel{TResult}"/> and <see cref="IMvxViewModel{TParameter}"/></typeparam>
    /// <typeparam name="TResult">Result awaited by Result Awaiting ViewModel and set by Result Setting ViewModel</typeparam>
    /// <returns>Boolean indicating successful navigation</returns>
    public static async Task<bool> NavigateRegisteringToResult<TViewModel, TParameter, TResult>(
        this IMvxNavigationService navigationService,
        IMvxResultAwaitingViewModel<TResult> fromViewModel,
        IMvxResultViewModelManager resultViewModelManager,
        TParameter parameter,
        IMvxBundle? presentationBundle = null,
        CancellationToken cancellationToken = default)
        where TViewModel : IMvxResultSettingViewModel<TResult>, IMvxViewModel<TParameter>
    {
        bool navigated = await navigationService.Navigate<TViewModel, TParameter>(parameter, presentationBundle, cancellationToken);
        if (navigated)
            fromViewModel.RegisterToResult(resultViewModelManager);
        return navigated;
    }

    /// <summary>
    /// Closes the View attached to the Result Setting ViewModel, with result
    /// </summary>
    /// <param name="navigationService">Navigation service</param>
    /// <param name="viewModel">Result Setting ViewModel to close</param>
    /// <param name="result">Result set by Result Setting ViewModel</param>
    /// <param name="cancellationToken">(optional) CancellationToken to cancel the closing</param>
    /// <typeparam name="TViewModel">Type of <see cref="IMvxResultSettingViewModel{TResult}"/></typeparam>
    /// <typeparam name="TResult">Result set by Result Setting ViewModel</typeparam>
    /// <returns></returns>
    public static async Task<bool> CloseSettingResult<TViewModel, TResult>(
        this IMvxNavigationService navigationService,
        TViewModel viewModel,
        TResult result,
        CancellationToken cancellationToken = default)
        where TViewModel : IMvxResultSettingViewModel<TResult>, IMvxViewModel
    {
        bool closed = await navigationService.Close(viewModel, cancellationToken);
        if (closed)
            viewModel.SetResult(result);
        return closed;
    }
}
