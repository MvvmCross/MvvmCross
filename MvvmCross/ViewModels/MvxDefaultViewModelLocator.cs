// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Navigation.EventArguments;

namespace MvvmCross.ViewModels
{
    public class MvxDefaultViewModelLocator
        : IMvxViewModelLocator
    {
        private IMvxNavigationService _navigationService;
        protected IMvxNavigationService NavigationService => _navigationService ?? (_navigationService = Mvx.IoCProvider.Resolve<IMvxNavigationService>());

        private IMvxLogProvider _logProvider;
        protected IMvxLogProvider LogProvider => _logProvider ?? (_logProvider = Mvx.IoCProvider.Resolve<IMvxLogProvider>());

        public MvxDefaultViewModelLocator() : this(null) { }

        public MvxDefaultViewModelLocator(IMvxNavigationService navigationService)
        {
            if (navigationService != null)
                _navigationService = navigationService;
        }

        public virtual IMvxViewModel Reload(IMvxViewModel viewModel,
                                            IMvxBundle parameterValues,
                                            IMvxBundle savedState, IMvxNavigateEventArgs navigationArgs)
        {
            RunViewModelLifecycle(viewModel, parameterValues, savedState, navigationArgs);

            return viewModel;
        }

        public virtual IMvxViewModel<TParameter> Reload<TParameter>(IMvxViewModel<TParameter> viewModel,
                                                                    TParameter param,
                                                                    IMvxBundle parameterValues,
                                                                    IMvxBundle savedState, IMvxNavigateEventArgs navigationArgs)
        {
            RunViewModelLifecycle(viewModel, param, parameterValues, savedState, navigationArgs);

            return viewModel;
        }

        public virtual IMvxViewModel Load(Type viewModelType,
                                          IMvxBundle parameterValues,
                                          IMvxBundle savedState, IMvxNavigateEventArgs navigationArgs)
        {
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

        public virtual IMvxViewModel<TParameter> Load<TParameter>(Type viewModelType,
                                                                  TParameter param,
                                                                  IMvxBundle parameterValues,
                                                                  IMvxBundle savedState, IMvxNavigateEventArgs navigationArgs)
        {
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

        protected virtual void CallCustomInitMethods(IMvxViewModel viewModel, IMvxBundle parameterValues)
        {
            viewModel.CallBundleMethods("Init", parameterValues);
        }

        protected virtual void CallReloadStateMethods(IMvxViewModel viewModel, IMvxBundle savedState)
        {
            viewModel.CallBundleMethods("ReloadState", savedState);
        }

        protected void RunViewModelLifecycle(IMvxViewModel viewModel, IMvxBundle parameterValues, IMvxBundle savedState, IMvxNavigateEventArgs navigationArgs)
        {
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

        protected void RunViewModelLifecycle<TParameter>(IMvxViewModel<TParameter> viewModel, TParameter param, IMvxBundle parameterValues, IMvxBundle savedState, IMvxNavigateEventArgs navigationArgs)
        {
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
}
