#region Copyright

// <copyright file="MvxViewModel.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModel;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.ViewModel
{
    public class MvxViewModel : MvxPropertyNotify, IMvxViewModel
    {
        protected MvxViewModel()
        {
            // nothing to do currently
        }

        public virtual void RequestStop()
        {
            // default behaviour does nothing!
        }

        protected bool RequestMainThreadAction(Action action)
        {
            if (ViewDispatcher != null)
                return ViewDispatcher.RequestMainThreadAction(action);

            return false;
        }

        public bool RequestNavigate<TViewModel>() where TViewModel : IMvxViewModel
        {
            return RequestNavigate<TViewModel>(null);
        }

        protected bool RequestNavigate<TViewModel>(object parameterValuesObject) where TViewModel : IMvxViewModel
        {
            return RequestNavigate<TViewModel>(parameterValuesObject.ToSimplePropertyDictionary());
        }

        protected bool RequestNavigate<TViewModel>(IDictionary<string, string> parameterValues)
            where TViewModel : IMvxViewModel
        {
            if (ViewDispatcher != null)
                return ViewDispatcher.RequestNavigate(new MvxShowViewModelRequest(
                                                          typeof (TViewModel),
                                                          parameterValues));

            return false;
        }

        protected bool RequestNavigateBack()
        {
            if (ViewDispatcher != null)
                return ViewDispatcher.RequestNavigateBack();

            return false;
        }
    }
}