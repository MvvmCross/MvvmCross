// MvxDefaultViewModelLocator.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.ViewModels
{
    public class MvxDefaultViewModelLocator
        : IMvxViewModelLocator
    {
        public virtual bool TryLoad(Type viewModelType,
            IMvxBundle parameterValues,
            IMvxBundle savedState,
            out IMvxViewModel viewModel)
        {
            viewModel = null;

            try
            {
                viewModel = (IMvxViewModel)Mvx.IocConstruct(viewModelType);
            }
            catch (Exception exception)
            {
                throw new MvxException(exception, string.Format("Problem creating viewModel of type {0} - problem {1}", viewModelType.Name, exception.ToLongString()));
            }

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
                throw new MvxException(exception, string.Format("Problem initialising viewModel of type {0} - problem {1}", viewModelType.Name, exception.ToLongString()));
            }

            return true;
        }

        protected virtual void CallCustomInitMethods(IMvxViewModel viewModel, IMvxBundle parameterValues)
        {
            viewModel.CallBundleMethods("Init", parameterValues);
        }

        protected virtual void CallReloadStateMethods(IMvxViewModel viewModel, IMvxBundle savedState)
        {
            viewModel.CallBundleMethods("ReloadState", savedState);
        }
    }
}