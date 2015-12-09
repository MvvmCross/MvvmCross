// MvxDefaultViewModelLocator.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using System;

namespace Cirrious.MvvmCross.ViewModels
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

        public virtual IMvxViewModel Load(Type viewModelType,
                                    IMvxBundle parameterValues,
                                    IMvxBundle savedState)
        {
            IMvxViewModel viewModel;
            try
            {
                viewModel = (IMvxViewModel)Mvx.IocConstruct(viewModelType);
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap("Problem creating viewModel of type {0}", viewModelType.Name);
            }

            RunViewModelLifecycle(viewModel, parameterValues, savedState);

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
                if (savedState != null)
                {
                    CallReloadStateMethods(viewModel, savedState);
                }
                viewModel.Start();
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap("Problem running viewModel lifecycle of type {0}", viewModel.GetType().Name);
            }
        }
    }
}