﻿using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Views.EventSource;
using Xamarin.Forms;

namespace MvvmCross.Forms.Views
{
    public class MvxEntryCell : MvxEventSourceEntryCell, IMvxElement
    {
        public MvxEntryCell()
        {
            this.AdaptForBinding();
        }

        public object DataContext
        {
            get
            {
                return BindingContext.DataContext;
            }
            set
            {
                base.BindingContext = value;
                BindingContext.DataContext = value;
            }
        }

        private IMvxBindingContext _bindingContext;
        public new IMvxBindingContext BindingContext
        {
            get
            {
                if (_bindingContext == null)
                    BindingContext = new MvxBindingContext(base.BindingContext);
                return _bindingContext;
            }
            set
            {
                _bindingContext = value;
            }
        }

        public IMvxViewModel ViewModel
        {
            get
            {
                return DataContext as IMvxViewModel;
            }
            set
            {
                DataContext = value;
                OnViewModelSet();
            }
        }

        protected virtual void OnViewModelSet()
        {
        }
    }

    public class MvxEntryCell<TViewModel>
        : MvxEntryCell
    , IMvxElement<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}