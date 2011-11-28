using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces;
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
            return RequestNavigate<TViewModel>(null, null);
        }

        protected bool RequestNavigate<TViewModel>(string actionName) where TViewModel : IMvxViewModel
        {
            return RequestNavigate<TViewModel>(actionName, null);
        }

        protected bool RequestNavigate<TViewModel>(string actionName, object parameterValuesObject) where TViewModel : IMvxViewModel
        {
            return RequestNavigate<TViewModel>(actionName, parameterValuesObject.ToSimplePropertyDictionary());
        }

        protected bool RequestNavigate<TViewModel>(object parameterValuesObject) where TViewModel : IMvxViewModel
        {
            return RequestNavigate<TViewModel>(null, parameterValuesObject.ToSimplePropertyDictionary());
        }

        protected bool RequestNavigate<TViewModel>(IDictionary<string, string> parameterValues) where TViewModel : IMvxViewModel
        {
            return RequestNavigate<TViewModel>(null, parameterValues);
        }

        protected bool RequestNavigate<TViewModel>(string actionName, IDictionary<string, string> parameterValues) where TViewModel : IMvxViewModel
        {
            if (ViewDispatcher != null)
                return ViewDispatcher.RequestNavigate(new MvxShowViewModelRequest(
                        new MxvViewModelAction<TViewModel>(actionName),
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