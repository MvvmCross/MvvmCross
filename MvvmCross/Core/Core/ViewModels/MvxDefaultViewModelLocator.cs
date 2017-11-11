// MvxDefaultViewModelLocator.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;

namespace MvvmCross.Core.ViewModels
{
    public class MvxDefaultViewModelLocator
        : IMvxViewModelLocator
    {
        public virtual IMvxViewModel Reload(IMvxViewModel viewModel,
                                            IMvxBundle parameterValues,
                                            IMvxBundle savedState)
        {
            RunViewModelLifecycle(viewModel, parameterValues, savedState);

            return viewModel;
        }

        public virtual IMvxViewModel<TParameter> Reload<TParameter>(IMvxViewModel<TParameter> viewModel,
                                                                    TParameter param,
                                                                    IMvxBundle parameterValues,
                                                                    IMvxBundle savedState)
        {
            RunViewModelLifecycle(viewModel, param, parameterValues, savedState);

            return viewModel;
        }

        public virtual IMvxViewModel Load(Type viewModelType,
                                          IMvxBundle parameterValues,
                                          IMvxBundle savedState)
        {
            IMvxViewModel viewModel;
            try
            {
                viewModel = (IMvxViewModel)Mvx.IocConstruct(viewModelType);
            }
            catch(Exception exception)
            {
                throw exception.MvxWrap("Problem creating viewModel of type {0}", viewModelType.Name);
            }

            RunViewModelLifecycle(viewModel, parameterValues, savedState);

            return viewModel;
        }

        public virtual IMvxViewModel<TParameter> Load<TParameter>(Type viewModelType,
                                                                  TParameter param,
                                                                  IMvxBundle parameterValues,
                                                                  IMvxBundle savedState)
        {
            IMvxViewModel<TParameter> viewModel;
            try
            {
                viewModel = (IMvxViewModel<TParameter>)Mvx.IocConstruct(viewModelType);
            }
            catch(Exception exception)
            {
                throw exception.MvxWrap("Problem creating viewModel of type {0}", viewModelType.Name);
            }

            RunViewModelLifecycle(viewModel, param, parameterValues, savedState);

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

        protected void RunViewModelLifecycle(IMvxViewModel viewModel, IMvxBundle parameterValues, IMvxBundle savedState)
        {
            try
            {
                CallCustomInitMethods(viewModel, parameterValues);
                if(savedState != null)
                {
                    CallReloadStateMethods(viewModel, savedState);
                }
                viewModel.Start();

                viewModel.Prepare();

                viewModel.InitializeTask = MvxNotifyTask.Create(() => viewModel.Initialize());
            }
            catch(Exception exception)
            {
                throw exception.MvxWrap("Problem running viewModel lifecycle of type {0}", viewModel.GetType().Name);
            }
        }

        protected void RunViewModelLifecycle<TParameter>(IMvxViewModel<TParameter> viewModel, TParameter param, IMvxBundle parameterValues, IMvxBundle savedState)
        {
            try
            {
                CallCustomInitMethods(viewModel, parameterValues);
                if(savedState != null)
                {
                    CallReloadStateMethods(viewModel, savedState);
                }
                viewModel.Start();

                viewModel.Prepare();
                viewModel.Prepare(param);

                viewModel.InitializeTask = MvxNotifyTask.Create(() => viewModel.Initialize());
            }
            catch(Exception exception)
            {
                throw exception.MvxWrap("Problem running viewModel lifecycle of type {0}", viewModel.GetType().Name);
            }
        }
    }
}