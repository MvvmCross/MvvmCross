// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Navigation.EventArguments;

namespace MvvmCross.ViewModels
{
#nullable enable
    /// <summary>
    /// ViewModelLocator helps locating and running start lifecycle of a ViewModel
    /// </summary>
    public interface IMvxViewModelLocator
    {
        /// <summary>
        /// Load ViewModel
        /// </summary>
        /// <param name="viewModelType"><see cref="Type"/> of ViewModel to load</param>
        /// <param name="parameterValues">Parameter values to pass into Init methods of ViewModel</param>
        /// <param name="savedState">Saved state to pass into RestoreState methods of ViewModel</param>
        /// <param name="navigationArgs">(Optional) Extra navigation arguments</param>
        /// <returns>Returns a ViewModel</returns>
        IMvxViewModel Load(
            Type viewModelType,
            IMvxBundle? parameterValues,
            IMvxBundle? savedState,
            IMvxNavigateEventArgs? navigationArgs = null);

        /// <summary>
        /// Load ViewModel with parameters
        /// </summary>
        /// <typeparam name="TParameter">Type of parameter to pass into <see cref="IMvxViewModel{TParameter}.Prepare(TParameter)"/> method</typeparam>
        /// <param name="viewModelType"><see cref="Type"/> of ViewModel to load</param>
        /// <param name="param">Parameters to pass into <see cref="IMvxViewModel{TParameter}.Prepare(TParameter)"/> method</param>
        /// <param name="parameterValues">Parameter values to pass into Init methods of ViewModel</param>
        /// <param name="savedState">Saved state to pass into RestoreState methods of ViewModel</param>
        /// <param name="navigationArgs">(Optional) Extra navigation arguments</param>
        /// <returns>Returns a ViewModel</returns>
        IMvxViewModel<TParameter> Load<TParameter>(
            Type viewModelType,
            TParameter param,
            IMvxBundle? parameterValues,
            IMvxBundle? savedState,
            IMvxNavigateEventArgs? navigationArgs = null)
            where TParameter : notnull;

        /// <summary>
        /// Reload ViewModel, runs start lifecycle in ViewModel.
        /// </summary>
        /// <param name="viewModel"><see cref="IMvxViewModel"/> to reload</param>
        /// <param name="parameterValues">Parameter values to pass into Init methods of ViewModel</param>
        /// <param name="savedState">Saved state to pass into RestoreState methods of ViewModel</param>
        /// <param name="navigationArgs">(Optional) Extra navigation arguments</param>
        /// <returns>Returns reloaded ViewModel</returns>
        IMvxViewModel Reload(
            IMvxViewModel viewModel,
            IMvxBundle? parameterValues,
            IMvxBundle? savedState,
            IMvxNavigateEventArgs? navigationArgs = null);

        /// <summary>
        /// Reload ViewModel, runs start lifecycle in ViewModel.
        /// </summary>
        /// <typeparam name="TParameter">Type of parameter to pass into <see cref="IMvxViewModel{TParameter}.Prepare(TParameter)"/> method</typeparam>
        /// <param name="viewModel"><see cref="IMvxViewModel"/> to reload</param>
        /// <param name="param">Parameters to pass into <see cref="IMvxViewModel{TParameter}.Prepare(TParameter)"/> method</param>
        /// <param name="parameterValues">Parameter values to pass into Init methods of ViewModel</param>
        /// <param name="savedState">Saved state to pass into RestoreState methods of ViewModel</param>
        /// <param name="navigationArgs">(Optional) Extra navigation arguments</param>
        /// <returns>Returns reloaded ViewModel</returns>
        IMvxViewModel<TParameter> Reload<TParameter>(
            IMvxViewModel<TParameter> viewModel,
            TParameter param,
            IMvxBundle? parameterValues,
            IMvxBundle? savedState,
            IMvxNavigateEventArgs? navigationArgs = null)
            where TParameter : notnull;
    }
#nullable restore
}
