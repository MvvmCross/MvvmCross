// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Exceptions;
using MvvmCross.Navigation.EventArguments;

namespace MvvmCross.ViewModels
{
#nullable enable
    /// <inheritdoc cref="IMvxViewModelLocator"/>
    public class MvxDefaultViewModelLocator
        : IMvxViewModelLocator
    {
        public virtual IMvxViewModel Load(
            Type viewModelType,
            IMvxBundle? parameterValues,
            IMvxBundle? savedState,
            IMvxNavigateEventArgs? navigationArgs = null)
        {
            if (viewModelType == null)
                throw new ArgumentNullException(nameof(viewModelType));

            IMvxViewModel viewModel;
            try
            {
                viewModel = (IMvxViewModel)Mvx.IoCProvider.IoCConstruct(viewModelType);
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap("Problem creating viewModel of type {0}", viewModelType.Name);
            }

            RunViewModelLifecycle(viewModel, parameterValues, savedState, navigationArgs);

            return viewModel;
        }

        public virtual IMvxViewModel<TParameter> Load<TParameter>(
            Type viewModelType,
            TParameter param,
            IMvxBundle? parameterValues,
            IMvxBundle? savedState,
            IMvxNavigateEventArgs? navigationArgs = null)
            where TParameter : notnull
        {
            if (viewModelType == null)
                throw new ArgumentNullException(nameof(viewModelType));

            IMvxViewModel<TParameter> viewModel;
            try
            {
                viewModel = (IMvxViewModel<TParameter>)Mvx.IoCProvider.IoCConstruct(viewModelType);
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap("Problem creating viewModel of type {0}", viewModelType.Name);
            }

            RunViewModelLifecycle(viewModel, param, parameterValues, savedState, navigationArgs);

            return viewModel;
        }

        public virtual IMvxViewModel Reload(
            IMvxViewModel viewModel,
            IMvxBundle? parameterValues,
            IMvxBundle? savedState,
            IMvxNavigateEventArgs? navigationArgs = null)
        {
            RunViewModelLifecycle(viewModel, parameterValues, savedState, navigationArgs);

            return viewModel;
        }

        public virtual IMvxViewModel<TParameter> Reload<TParameter>(
            IMvxViewModel<TParameter> viewModel,
            TParameter param,
            IMvxBundle? parameterValues,
            IMvxBundle? savedState,
            IMvxNavigateEventArgs? navigationArgs = null)
            where TParameter : notnull
        {
            RunViewModelLifecycle(viewModel, param, parameterValues, savedState, navigationArgs);

            return viewModel;
        }

        protected virtual void CallCustomInitMethods(IMvxViewModel viewModel, IMvxBundle? parameterValues)
        {
            viewModel.CallBundleMethods("Init", parameterValues);
        }

        protected virtual void CallReloadStateMethods(IMvxViewModel viewModel, IMvxBundle? savedState)
        {
            viewModel.CallBundleMethods("ReloadState", savedState);
        }

        protected void RunViewModelLifecycle(
            IMvxViewModel viewModel,
            IMvxBundle? parameterValues,
            IMvxBundle? savedState,
            IMvxNavigateEventArgs? navigationArgs)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            try
            {
                CallCustomInitMethods(viewModel, parameterValues);
                if (navigationArgs?.Cancel == true)
                    return;
                if (savedState != null)
                {
                    CallReloadStateMethods(viewModel, savedState);
                    if (navigationArgs?.Cancel == true)
                        return;
                }
                viewModel.Start();
                if (navigationArgs?.Cancel == true)
                    return;

                viewModel.Prepare();
                if (navigationArgs?.Cancel == true)
                    return;

                viewModel.InitializeTask = MvxNotifyTask.Create(() => viewModel.Initialize());
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap("Problem running viewModel lifecycle of type {0}", viewModel.GetType().Name);
            }
        }

        protected void RunViewModelLifecycle<TParameter>(
            IMvxViewModel<TParameter> viewModel,
            TParameter param,
            IMvxBundle? parameterValues,
            IMvxBundle? savedState,
            IMvxNavigateEventArgs? navigationArgs)
            where TParameter : notnull
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            try
            {
                CallCustomInitMethods(viewModel, parameterValues);
                if (navigationArgs?.Cancel == true)
                    return;
                if (savedState != null)
                {
                    CallReloadStateMethods(viewModel, savedState);
                    if (navigationArgs?.Cancel == true)
                        return;
                }
                viewModel.Start();
                if (navigationArgs?.Cancel == true)
                    return;

                viewModel.Prepare();
                if (navigationArgs?.Cancel == true)
                    return;

                viewModel.Prepare(param);
                if (navigationArgs?.Cancel == true)
                    return;

                viewModel.InitializeTask = MvxNotifyTask.Create(() => viewModel.Initialize());
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap("Problem running viewModel lifecycle of type {0}", viewModel.GetType().Name);
            }
        }
    }
#nullable restore
}
